using InvSys.Application.Common;
using InvSys.Application.ValueObjects;

namespace InvSys.Application.Entities;

public class Product : AuditableEntity, IHasDomainEvent
{
    public string Name { get; private set; }

    // TODO: create value object for this
    public SKU SKU { get; private set; }
    public string Condition { get; private set; }
    public string Location { get; private set; }
    public int AvailableQuantity { get; private set; }
    public int StockQuantity { get; private set; }

    // TODO: create value object for this
    public Price Price { get; private set; }

    public List<DomainEvent> DomainEvents => new List<DomainEvent>();

    private Product(string name,
                    SKU sku,
                    string condition,
                    string location,
                    int availableQuantity,
                    int stockQuantity,
                    Price price,
                    DateTime createdDateTime,
                    DateTime updatedDateTime) : base(createdDateTime, updatedDateTime)
    {
        Name = name;
        SKU = sku;
        Condition = condition;
        Location = location;
        AvailableQuantity = availableQuantity;
        StockQuantity = stockQuantity;
        Price = price;
    }

    public Product Create(string name,
                          SKU sku,
                          string condition,
                          string location,
                          int availableQuantity,
                          int stockQuantity,
                          Price price)
    {
        var product = new Product(
                name,
                sku,
                condition,
                location,
                availableQuantity,
                stockQuantity,
                price,
                DateTime.UtcNow,
                DateTime.UtcNow
                );

        DomainEvents.Add(new ProductCreatedEvent(this));

        return product;
    }
}

public class ProductCreatedEvent : DomainEvent
{
    public ProductCreatedEvent(Product product)
    {
        Product = product;
    }

    public Product Product { get; }
}