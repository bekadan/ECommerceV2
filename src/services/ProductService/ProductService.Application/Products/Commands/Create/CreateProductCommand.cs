using ProductService.Application.Core.Abstractions.Messaging;
using ProductService.Domain.Entities;

namespace ProductService.Application.Products.Commands.Create
{
    public sealed record CreateProductCommand() : ICommand<Product>;
}
