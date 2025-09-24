using ProductService.Application.Core.Abstractions.Messaging;
using ProductService.Application.Core.Abstractions.Repositories;
using ProductService.Domain.Entities;

namespace ProductService.Application.Products.Commands.Create;

public sealed class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, Product>
{
    private readonly IProductRepository _repository;

    public CreateProductCommandHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public Task<Product> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
