using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.HeroCassandraComponent.Queries;

public record GetByIdHeroQuery : IRequest<Hero?>
{
    public Guid Id { get; set; }
}

public class GetByIdHeroQueryHandler : IRequestHandler<GetByIdHeroQuery, Hero?>
{
    private readonly ICassandraNeroRepository _repository;

    public GetByIdHeroQueryHandler(ICassandraNeroRepository repository)
    {
        _repository = repository;
    }

    public async Task<Hero?> Handle(GetByIdHeroQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetByIdAsync(request.Id);
    }
}