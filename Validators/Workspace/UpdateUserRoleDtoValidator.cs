using FluentValidation;
using TaskMate.DTOs.Workspace;

public class UpdateUserRoleDtoValidator : AbstractValidator<UpdateUserRoleDto>
{
    public UpdateUserRoleDtoValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("WorkspaceId is required.");
        RuleFor(x => x.WorkspaceId).NotEmpty().WithMessage("WorkspaceId is required.");
    }
}