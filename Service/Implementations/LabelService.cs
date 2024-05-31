using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaskMate.Context;
using TaskMate.DTOs.Card;
using TaskMate.DTOs.Label;
using TaskMate.Entities;
using TaskMate.Service.Abstraction;

namespace TaskMate.Service.Implementations;

public class LabelService : ILabelService
{
    private readonly IMapper _mapper;
    private readonly AppDbContext _appDbContext;

    public LabelService(IMapper mapper, AppDbContext appDbContext)
    {
        _mapper = mapper;
        _appDbContext = appDbContext;
    }


    public async Task CreateAsync(CreateLabelDto createLabelDtos)
    {
        var newLabel = _mapper.Map<Labels>(createLabelDtos);

        await _appDbContext.Labels.AddAsync(newLabel);
        await _appDbContext.SaveChangesAsync();
        LabelCard labelCard = new() 
        {
            LabelId = newLabel.Id,
            CardId = createLabelDtos.CardId
        };

        await _appDbContext.LabelCards.AddAsync(labelCard); 
        await _appDbContext.SaveChangesAsync();
    }
}
