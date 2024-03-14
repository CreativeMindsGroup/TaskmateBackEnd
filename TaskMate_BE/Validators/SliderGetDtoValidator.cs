using FluentValidation;
using TaskMate.DTOs.Slider;
namespace TaskMate.Validators;

public class SliderGetDtoValidator : AbstractValidator<SliderCreateDTO>
{
    public SliderGetDtoValidator()
    {
        RuleFor(x => x.Name).NotNull().NotEmpty().MaximumLength(12000);
    }
}