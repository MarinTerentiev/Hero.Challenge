using Domain.Enums;
using FluentValidation;

namespace Application.HeroCassandraComponent.Commands;

public class AddHeroCommandValidator : AbstractValidator<AddHeroCommand>
{
    public AddHeroCommandValidator()
    {

        RuleFor(x => x.Hero.Name).MaximumLength(50).NotEmpty();
        RuleFor(x => x.Hero.Weapon).Must(x => Enum.IsDefined(typeof(Weapon), x)).WithMessage("Invalid weapon value.");
    }
}
