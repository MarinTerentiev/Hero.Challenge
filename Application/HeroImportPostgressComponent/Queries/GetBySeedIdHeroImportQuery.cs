using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.HeroImportPostgressComponent.Queries;

public record GetBySeedIdHeroImportQuery : IRequest<IEnumerable<HeroImport>>
{
    public Guid SeedId { get; set; }
}

public class GetBySeedIdHeroImportQueryHandler : IRequestHandler<GetBySeedIdHeroImportQuery, IEnumerable<HeroImport>>
{
    private readonly IPostgressHeroImportRepository _repository;

    public GetBySeedIdHeroImportQueryHandler(IPostgressHeroImportRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<HeroImport>> Handle(GetBySeedIdHeroImportQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetHeroesBySeedIdAsync(request.SeedId);
    }
}
