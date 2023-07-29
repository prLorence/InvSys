using InvSys.Application.Common;
using InvSys.Application.Common.Models;
using InvSys.Application.ValueObjects;

namespace InvSys.Application.Entities;

public sealed class Product : AuditableEntity<ProductId>, IHasDomainEvent
{
    public string Name { get; private set; }
    public SKU SKU { get; private set; }
    public string Condition { get; private set; }
    public ProductLocation Location { get; private set; }
    public ProductQuantity AvailableQuantity { get; private set; }
    public ProductQuantity StockQuantity { get; private set; }
    public ProductPrice Price { get; private set; }

    public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();

    public DateTime CreatedDateTime { get; private set; }

    public DateTime UpdatedDateTime { get; }

#pragma warning disable CS8618
    private Product()
    {
    }
#pragma warning restore CS8618

    private Product(
        ProductId productId,
        string name,
        SKU sku,
        string condition,
        ProductLocation location,
        ProductQuantity availableQuantity,
        ProductQuantity stockQuantity,
        ProductPrice price,
        DateTime createdDateTime,
        DateTime updatedDateTime) : base(productId)
    {
        Name = name;
        SKU = sku;
        Condition = condition;
        Location = location;
        AvailableQuantity = availableQuantity;
        StockQuantity = stockQuantity;
        Price = price;
        CreatedDateTime = createdDateTime;
        UpdatedDateTime = updatedDateTime;
    }

    public static Product Create(
                          string name,
                          string sku,
                          string condition,
                          ProductLocation location,
                          int availableQuantity,
                          int stockQuantity,
                          double price)
    {
        return new Product(
                ProductId.CreateUnique(),
                name,
                SKU.Create(sku),
                condition,
                location,
                ProductQuantity.Create(availableQuantity),
                ProductQuantity.Create(stockQuantity),
                ProductPrice.Create(price),
                DateTime.UtcNow,
                DateTime.UtcNow);
    }
}

public sealed class ProductId : EntityId<Guid>
{
    public override Guid Value { get; protected set; }

    private ProductId(Guid value)
    {
        Value = value;
    }

    public static ProductId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    public static ProductId Create(Guid value)
    {
        return new(value);
    }


    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

public enum ProductLocation
{
    WAREHOUSE1 = 0,
    WAREHOUSE2 = 1,
    WAREHOUSE3 = 2
}