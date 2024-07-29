using Domain.Enums;
using FluentValidation;

namespace Application.HeroCassandraComponent.Commands
{
    public class UpdateHeroCommandValidation : AbstractValidator<UpdateHeroCommand>
    {
        public UpdateHeroCommandValidation()
        {

            RuleFor(x => x.Hero.Name).MaximumLength(50).NotEmpty();
            RuleFor(x => x.Hero.Weapon).Must(x => Enum.IsDefined(typeof(Weapon), x)).WithMessage("Invalid weapon value.");
        }
    }
}
