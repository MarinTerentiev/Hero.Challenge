using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.HeroComponent.Commands;

public record AddHeroCommand : IRequest
{
    public required Hero Hero { get; set; }
}

public class AddHeroCommandHandler : IRequestHandler<AddHeroCommand>
{
    private readonly ICassandraNeroRepository _repository;

    public AddHeroCommandHandler(ICassandraNeroRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(AddHeroCommand request, CancellationToken cancellationToken)
    {
        await _repository.AddAsync(request.Hero);
    }
}
