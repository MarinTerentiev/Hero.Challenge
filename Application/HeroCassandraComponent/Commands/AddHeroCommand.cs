using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.HeroCassandraComponent.Commands;

public record AddHeroCommand : IRequest<Guid>
{
    public required Hero Hero { get; set; }
}

public class AddHeroCommandHandler : IRequestHandler<AddHeroCommand, Guid>
{
    private readonly ICassandraNeroRepository _repository;

    public AddHeroCommandHandler(ICassandraNeroRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(AddHeroCommand request, CancellationToken cancellationToken)
    {
        request.Hero.Id = Guid.NewGuid();
        await _repository.AddAsync(request.Hero);
        return request.Hero.Id;
    }
}
