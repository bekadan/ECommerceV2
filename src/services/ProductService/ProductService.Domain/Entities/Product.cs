using ProductService.Domain.Abstractions;
using ProductService.Domain.Primitives;
using ProductService.Domain.ValueObjects;

namespace ProductService.Domain.Entities;

public sealed class Product : Entity, ISoftDeletableEntity, IAuditableEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Price Price { get; set; }
    public int Stock { get; set; }

    public DateTime? DeletedOnUtc { get; }

    public bool Deleted { get; }

    public DateTime CreatedOnUtc { get; }

    public DateTime? ModifiedOnUtc { get; }

    public Product(Guid id, string name, string description, Price price, int stock)
    {
        Id = id;
        Name = name;
        Description = description;
        Price = price;
        Stock = stock;
    }

    // EF Core
    public Product()
    {
        
    }
}
