using Application.Common.Interfaces;
using MediatR;

namespace Application.HeroCassandraComponent.Commands;

public record DeleteHeroCommand : IRequest
{
    public Guid Id { get; set; }
}

public class DeleteHeroCommandHandler : IRequestHandler<DeleteHeroCommand>
{
    private readonly ICassandraNeroRepository _repository;

    public DeleteHeroCommandHandler(ICassandraNeroRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteHeroCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.Id);
    }
}
