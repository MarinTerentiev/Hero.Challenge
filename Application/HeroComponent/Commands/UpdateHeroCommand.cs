using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.HeroComponent.Commands;

public record UpdateHeroCommand : IRequest
{
    public required Hero Hero { get; set; }
}

public class UpdateHeroCommandHandler : IRequestHandler<UpdateHeroCommand>
{
    private readonly ICassandraNeroRepository _repository;

    public UpdateHeroCommandHandler(ICassandraNeroRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(UpdateHeroCommand request, CancellationToken cancellationToken)
    {
        await _repository.UpdateAsync(request.Hero);
    }
}
