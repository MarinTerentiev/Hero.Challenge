using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.HeroImportPostgressComponent.Commands;

public record BulkInsertHeroImportCommand : IRequest
{
    public required List<HeroImport> Heroes { get; set; }
}


public class BulkInsertHeroImportCommandHandler : IRequestHandler<BulkInsertHeroImportCommand>
{
    private readonly IPostgressHeroImportRepository _repository;

    public BulkInsertHeroImportCommandHandler(IPostgressHeroImportRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(BulkInsertHeroImportCommand request, CancellationToken cancellationToken)
    {
        await _repository.BulkInsertHeroesAsync(request.Heroes);
    }
}
