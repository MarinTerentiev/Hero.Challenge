using FluentValidation;

namespace Application.HeroComponent.Commands;

public class AddHeroCommandValidator : AbstractValidator<AddHeroCommand>
{
    public AddHeroCommandValidator()
    {

        RuleFor(x => x.Hero.Name).MaximumLength(50).NotEmpty();
    }
}
