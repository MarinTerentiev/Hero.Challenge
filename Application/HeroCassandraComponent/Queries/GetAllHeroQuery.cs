using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.HeroCassandraComponent.Queries;

public record GetAllHeroQuery : IRequest<IEnumerable<Hero>>;

public class GetAllHeroQueryHandler : IRequestHandler<GetAllHeroQuery, IEnumerable<Hero>>
{
    private readonly ICassandraNeroRepository _repository;

    public GetAllHeroQueryHandler(ICassandraNeroRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Hero>> Handle(GetAllHeroQuery request, CancellationToken cancellationToken)
    {
        return _repository.GetAllAsync();
    }
}
