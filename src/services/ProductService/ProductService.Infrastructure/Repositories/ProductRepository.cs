using Microsoft.EntityFrameworkCore;
using ProductService.Application.Core.Abstractions.Data;
using ProductService.Application.Core.Abstractions.Repositories;
using ProductService.Domain.Entities;

namespace ProductService.Infrastructure.Repositories;

public sealed class ProductRepository : IProductRepository
{
    private readonly IDbContext _dbContext;

    public ProductRepository(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Create(Product product)
        => _dbContext.Set<Product>().Add(product);

    public async Task DeleteAsync(Guid id)
        => await _dbContext.Set<Product>().Where(p => p.Id == id).ExecuteDeleteAsync();

    public async Task<List<Product>> GetAllsAsync()
        => await _dbContext.Set<Product>().ToListAsync();

    public async Task<Product?> GetByIdAsync(Guid id)
        => await _dbContext.Set<Product>().FirstOrDefaultAsync(p => p.Id == id);

    public Product? Update(Product product)
        => _dbContext.Set<Product>().Update(product).Entity;
}
