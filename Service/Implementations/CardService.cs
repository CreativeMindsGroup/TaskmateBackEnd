using System.IO.Compression;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TaskMate.Context;
using TaskMate.DTOs.Card;
using TaskMate.DTOs.CardMembers;
using TaskMate.DTOs.Checkitem;
using TaskMate.Entities;
using TaskMate.Exceptions;
using TaskMate.Helper.Enum.User;
using TaskMate.Service.Abstraction;
using CardAttachment = TaskMate.Entities.CardAttachment;

namespace TaskMate.Service.Implementations
{
    public class CardService : ICardService
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public CardService(AppDbContext appDbContext, UserManager<AppUser> userManager, IMapper mapper, IConfiguration configuration)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
            _mapper = mapper;
            _configuration = configuration;
        }
        public async Task<bool> CheckUserAdminRoleInWorkspace(string userId, Guid workspaceId, bool isMemberAllowed)
        {
            var user = await _appDbContext.WorkspaceUsers
                        .FirstOrDefaultAsync(wu => wu.WorkspaceId == workspaceId && wu.AppUserId == userId);

            if (user == null)
            {
                throw new NotFoundException("User not found in workspace!");
            }

            if (Enum.TryParse<Role>(user.Role, true, out var roleEnum))
            {
                if (roleEnum == Role.GlobalAdmin || roleEnum == Role.Admin)
                {
                    return true;
                }
                else if (roleEnum == Role.Member && isMemberAllowed)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                throw new ArgumentException("The role value in the database is undefined in the Role enum.");
            }
        }
        public async Task AddCardDateAsync(CardAddDatesDto cardAddDatesDto)
        {
            if (cardAddDatesDto.StartDate == null || cardAddDatesDto.EndDate == null)
                throw new Exception("Start Date and End Date must be mentioned");

            if (cardAddDatesDto.StartDate > cardAddDatesDto.EndDate)
                throw new Exception("Start Date cannot be greater than End Date");

            var card = await _appDbContext.Cards.FindAsync(cardAddDatesDto.CardId);
            if (card == null)
                throw new NotFoundException("Card not found");

            card.StartDate = cardAddDatesDto.StartDate;
            card.EndDate = cardAddDatesDto.EndDate;

            _appDbContext.Cards.Update(card);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task CreateAsync(CreateCardDto createCardDto)
        {
            if (!await _appDbContext.CardLists.AnyAsync(x => x.Id == createCardDto.CardListId))
                throw new NotFoundException("Card List not found");

            // Get the latest order number
            var maxOrder = await _appDbContext.Cards
                                              .Where(x => x.CardListId == createCardDto.CardListId)
                                              .MaxAsync(x => (int?)x.Order) ?? 0;

            var newCard = _mapper.Map<Card>(createCardDto);
            newCard.Order = maxOrder + 1;  // Set the order

            await _appDbContext.Cards.AddAsync(newCard);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task DragAndDrop(DragAndDropCardDto dragAndDropCardDto)
        {
            var card = await _appDbContext.Cards.FindAsync(dragAndDropCardDto.CardId);
            if (card == null)
                throw new NotFoundException("Card not found");

            // Adjust the order of the card being moved
            var targetCardList = await _appDbContext.CardLists
                .Include(cl => cl.Cards)
                .FirstOrDefaultAsync(cl => cl.Id == dragAndDropCardDto.CardListId);

            if (targetCardList == null)
                throw new NotFoundException("Target Card List not found");

            var maxOrder = targetCardList.Cards.Max(c => (int?)c.Order) ?? 0;
            card.Order = maxOrder + 1;
            card.CardListId = dragAndDropCardDto.CardListId;

            _appDbContext.Cards.Update(card);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<GetCardDto> GetByIdAsync(Guid cardId)
        {
            var card = await _appDbContext.Cards.FindAsync(cardId);
            if (card == null)
                throw new NotFoundException("Card not found");

            return _mapper.Map<GetCardDto>(card);
        }


        public async Task Remove(string appUserId, Guid cardId, Guid WorkspaceId)
        {
            var Result = CheckUserAdminRoleInWorkspace(appUserId, WorkspaceId,false);
            if (await Result == false)
                throw new NotFoundException("No Access");
            var card = await _appDbContext.Cards.FindAsync(cardId);
            if (card == null)
                throw new NotFoundException("Card not found");

            var cardListId = card.CardListId;
            var order = card.Order;

            _appDbContext.Cards.Remove(card);
            await _appDbContext.SaveChangesAsync();

            // Update the order of the remaining cards in the same list
            var remainingCards = await _appDbContext.Cards
                .Where(c => c.CardListId == cardListId && c.Order > order)
                .ToListAsync();

            foreach (var remainingCard in remainingCards)
            {
                remainingCard.Order--;
            }

            await _appDbContext.SaveChangesAsync();
        }

        public async Task<List<GetCardDto>> GetAllCardsByBoardIdAsync(Guid boardId)
        {
            var cards = await _appDbContext.Cards
                .Include(c => c.CardList)
                .Include(c => c.CustomFields)
                .Where(c => c.CardList.BoardsId == boardId)
                .OrderBy(c => c.Order)
                .ToListAsync();

            return _mapper.Map<List<GetCardDto>>(cards);
        }

        public async Task<List<GetCardDto>> GetAllCardsByCardListIdAsync(Guid cardListId)
        {
            var cards = await _appDbContext.Cards
                .Where(c => c.CardListId == cardListId)
                .OrderBy(c => c.Order)
                .ToListAsync();

            return _mapper.Map<List<GetCardDto>>(cards);
        }

        public async Task UpdateAsync(UpdateCardDto updateCardDto)
        {
            var card = await _appDbContext.Cards.FindAsync(updateCardDto.CardId);
            if (card == null)
                throw new NotFoundException("Card not found");

            card.Title = updateCardDto.Title;
            card.Description = updateCardDto.Description;

            _appDbContext.Cards.Update(card);
            await _appDbContext.SaveChangesAsync();
        }


        private async Task<bool> CheckAdminAsync(string appUserId)
        {
            var user = await _userManager.FindByIdAsync(appUserId);
            var roles = await _userManager.GetRolesAsync(user);

            return roles.Contains(Role.GlobalAdmin.ToString()) || roles.Contains(Role.Admin.ToString());
        }

        public async Task ReorderCardsAsync(ReorderCardsDto reorderCardsDto)
        {
            foreach (var cardOrder in reorderCardsDto.CardOrders)
            {
                var card = await _appDbContext.Cards.FindAsync(cardOrder.CardId);
                if (card != null)
                {
                    card.Order = cardOrder.Order;
                    _appDbContext.Cards.Update(card);
                }
            }
            await _appDbContext.SaveChangesAsync();
        }
        public async Task UpdateCardDescriptionAsync(UpdateCardDescriptionDto updateCardDescriptionDto)
        {
            var Result = CheckUserAdminRoleInWorkspace(updateCardDescriptionDto.UserId.ToString(), updateCardDescriptionDto.WorkspcaeId, true);
            if (await Result == false)
                throw new NotFoundException("No Access");
            var card = await _appDbContext.Cards.FindAsync(updateCardDescriptionDto.CardId);
            if (card == null)
                throw new NotFoundException("Card not found");

            card.Description = updateCardDescriptionDto.Description;

            _appDbContext.Cards.Update(card);
            await _appDbContext.SaveChangesAsync();
        }
        public async Task ChangeCoverColorAsync(CardCoverCreateDto Dto)
        {
            var Result = CheckUserAdminRoleInWorkspace(Dto.AdminId.ToString(), Dto.WorkspaceId,false);
            if (await Result == false)
                throw new NotFoundException("No Access");
            var card = await _appDbContext.Cards.FindAsync(Dto.CardId);
            if (card == null)
                throw new NotFoundException("Card not found");

            card.CoverColor = Dto.Color;

            _appDbContext.Cards.Update(card);
            await _appDbContext.SaveChangesAsync();
        }
        public async Task AddOrUpdateCardCover(CardCoverCreateDto Dto)
        {
            var card = await _appDbContext.Cards.FirstOrDefaultAsync(x => x.Id == Dto.CardId);
            if (card is null) throw new NotFoundException("Not Found");
            card.CoverColor = Dto.Color;
            await _appDbContext.SaveChangesAsync();
        }

        public async Task UpdateDueDate(CreateCardDueDateDto updateCheckitemDto)
        {
            var Result = CheckUserAdminRoleInWorkspace(updateCheckitemDto.UserId.ToString(), updateCheckitemDto.WorkspaceId,true);
            if (await Result == false)
                throw new NotFoundException("No Access");

            var Card = await _appDbContext.Cards.FirstOrDefaultAsync(x => x.Id == updateCheckitemDto.CardId);
            if (Card is null)
                throw new NotFoundException("Not Found");
            Card.DueDate = updateCheckitemDto.DueDate;
            await _appDbContext.SaveChangesAsync();
        }
        public async Task DueDateDone([FromBody] UpdateCardDueDateDto Dto)
        {
            var Card = await _appDbContext.Cards.FirstOrDefaultAsync(x => x.Id == Dto.CardId);
            if (Card is null)
                throw new NotFoundException("Not Found");

            if (Dto.isDueDateDone != null)
            {
                Card.isDueDateDone = Dto.isDueDateDone;
            }
            if (Dto.DueDate != null)
            {
                Card.DueDate = Dto.DueDate;
            }
            await _appDbContext.SaveChangesAsync();
        }
        public async Task UploadAttachmentAsync(FileUploadDto uploadDto, IFormFile UploadFile)
        {
            var Result = CheckUserAdminRoleInWorkspace(uploadDto.UserId, uploadDto.WorkspaceId, true);
            if (await Result == false)
                throw new NotFoundException("No Access");
            var card = await _appDbContext.Cards
                              .Include(c => c.Attachments)
                              .FirstOrDefaultAsync(c => c.Id == uploadDto.CardId);
            if (card == null)
                throw new NotFoundException("Card not found");

            var file = UploadFile;
            if (file != null)
            {
                string originalExtension = Path.GetExtension(file.FileName);

                string fileNameToUse = string.IsNullOrEmpty(uploadDto.FileName)
                                       ? file.FileName
                                       : uploadDto.FileName + (string.IsNullOrEmpty(Path.GetExtension(uploadDto.FileName)) ? originalExtension : "");
                var path = _configuration.GetValue<string>("FileSettings:FileUpload");
                var filePath = Path.Combine(path, fileNameToUse);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                card.Attachments.Add(new CardAttachment
                {
                    FileName = fileNameToUse,
                    FilePath = filePath,
                    CardId = card.Id
                });

                await _appDbContext.SaveChangesAsync();
            }
        }

        public async Task<List<CardAttachmentDto>> GetUploads(Guid CardId)
        {
            var attachments = await _appDbContext.CardAttachments
                .Where(a => a.CardId == CardId)
                .ToListAsync();
            var attachmentDtos = _mapper.Map<List<CardAttachmentDto>>(attachments);
            return attachmentDtos;
        }
        public async Task DeleteAttachment(Guid attachmentId, string userId, Guid workspaceId)
        {
            var result = await CheckUserAdminRoleInWorkspace(userId, workspaceId,false);
            if (!result)
                throw new NotFoundException("No Access");

            var attachment = await _appDbContext.CardAttachments
                .FirstOrDefaultAsync(a => a.Id == attachmentId);

            if (attachment == null)
                throw new NotFoundException("Attachment not found!");

            // Delete the file from the file system
            if (File.Exists(attachment.FilePath))
            {
                File.Delete(attachment.FilePath);
            }

            _appDbContext.CardAttachments.Remove(attachment);
            await _appDbContext.SaveChangesAsync();
        }
        public async Task<IActionResult> DownloadFileAsync(Guid cardId, string fileName)
        {
            var attachment = await _appDbContext.CardAttachments
                .Where(a => a.CardId == cardId && a.FileName == fileName)
                .FirstOrDefaultAsync();

            if (attachment == null)
                return new NotFoundResult();

            var filePath = attachment.FilePath;
            if (!System.IO.File.Exists(filePath))
                return new NotFoundResult();

            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            return new FileContentResult(fileBytes, "application/octet-stream")
            {
                FileDownloadName = fileName
            };
        }

        public async Task<MemoryStream> GetFiles(Guid cardId)
        {
            var attachments = await _appDbContext.CardAttachments
                .Where(a => a.CardId == cardId)
                .ToListAsync();

            if (attachments == null || attachments.Count == 0)
                return null; // Or handle it differently based on your requirements

            var zipMemoryStream = new MemoryStream();
            using (var archive = new ZipArchive(zipMemoryStream, ZipArchiveMode.Create, true))
            {
                foreach (var attachment in attachments)
                {
                    var filePath = attachment.FilePath;
                    if (!System.IO.File.Exists(filePath))
                        continue;

                    var fileName = Path.GetFileName(filePath);
                    var entry = archive.CreateEntry(fileName);
                    using (var entryStream = entry.Open())
                    using (var fileStream = System.IO.File.OpenRead(filePath))
                    {
                        await fileStream.CopyToAsync(entryStream);
                    }
                }
            }

            zipMemoryStream.Seek(0, SeekOrigin.Begin);
            return zipMemoryStream;
        }

        public async Task MakeArchive(MakeArchiveDto dto)
        {
            var Result = CheckUserAdminRoleInWorkspace(dto.AdminId.ToString(), dto.WorkspaceId, false);
            if (await Result == false)
                throw new NotFoundException("No Access");
            var card = await _appDbContext.Cards.FindAsync(dto.CardId);
            if (card == null)
                throw new NotFoundException("Card not found");
            card.isArchived = dto.isArchived;
            _appDbContext.SaveChanges();
        }
        public async Task UpdateTitle(UpdateTitleDto dto)
        {
            var Result = CheckUserAdminRoleInWorkspace(dto.AdminId.ToString(), dto.WorkspaceId,false);
            if (await Result == false)
                throw new NotFoundException("No Access");
            var card = await _appDbContext.Cards.FindAsync(dto.Id);
            if (card == null)
                throw new NotFoundException("Card not found");
            card.Title = dto.Title;
            await _appDbContext.SaveChangesAsync();
        }
        public async Task<List<GetCardDto>> getAllArchivedCards(Guid boardId)
        {
            var cards = await _appDbContext.Cards
                .Include(c => c.CardList)
                .Where(c => c.CardList.BoardsId == boardId)
                .OrderBy(c => c.Order)
                .Where(c => c.isArchived == true)
                .ToListAsync();
            return _mapper.Map<List<GetCardDto>>(cards);
        }

        public async Task<bool> AddUserToCard(AddMemberToCardDto dto)
        {
            var hasAccess = await CheckUserAdminRoleInWorkspace(dto.AdminId.ToString(), dto.WorkspaceId, false);
            if (!hasAccess)
                throw new NotFoundException("No Access");

            var card = await _appDbContext.Cards
                .Include(c => c.AppUsersCards) // Ensure AppUsersCards collection is included in the query
                .FirstOrDefaultAsync(x => x.Id == dto.CardId);
            if (card == null)
                throw new NotFoundException("Card not found");

            var appUser = await _appDbContext.AppUsers
                .Include(u => u.AppUsersCards) // Ensure AppUsersCards collection is included in the query
                .FirstOrDefaultAsync(x => x.Id == dto.MemberId.ToString());
            if (appUser == null)
                throw new NotFoundException("User not found");

            // Check if the user is already in the card's AppUsers
            if (!card.AppUsersCards.Any(uc => uc.AppUserId == appUser.Id))
            {
                var appUsersCards = new AppUsersCards
                {
                    AppUserId = appUser.Id,
                    CardId = card.Id
                };
                card.AppUsersCards.Add(appUsersCards);
                await _appDbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }


        public async Task<bool> RemoveUserFromCard(AddMemberToCardDto dto)
        {
            var hasAccess = await CheckUserAdminRoleInWorkspace(dto.AdminId.ToString(), dto.WorkspaceId, false);
            if (!hasAccess)
                throw new NotFoundException("No Access");

            var card = await _appDbContext.Cards
                .Include(c => c.AppUsersCards)
                .FirstOrDefaultAsync(x => x.Id == dto.CardId);
            var appUser = await _appDbContext.AppUsers
                .Include(u => u.AppUsersCards)
                .FirstOrDefaultAsync(x => x.Id == dto.MemberId.ToString());

            if (card == null || appUser == null)
                return false;

            var appUserCard = card.AppUsersCards.FirstOrDefault(uc => uc.AppUserId == appUser.Id && uc.CardId == card.Id);
            if (appUserCard != null)
            {
                card.AppUsersCards.Remove(appUserCard);
                appUser.AppUsersCards.Remove(appUserCard); 
                await _appDbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

    }

}
