using ProductService.Domain.Entities;

namespace ProductService.Application.Core.Abstractions.Repositories;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id);
    Task<List<Product>> GetAllsAsync();
    void Create(Product product);
    Product? Update(Product product);
    Task DeleteAsync(Guid id);
}
