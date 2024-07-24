using FluentValidation;

namespace Application.HeroComponent.Commands
{
    public class UpdateHeroCommandValidation : AbstractValidator<UpdateHeroCommand>
    {
        public UpdateHeroCommandValidation()
        {

            RuleFor(x => x.Hero.Name).MaximumLength(50).NotEmpty();
        }
    }
}
