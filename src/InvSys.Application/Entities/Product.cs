using InvSys.Application.Common;
using InvSys.Application.Common.Models;
using InvSys.Application.ValueObjects;

namespace InvSys.Application.Entities;

public sealed class Product : AuditableEntity<ProductId>, IHasDomainEvent
{
    public string Name { get; set; }
    public SKU SKU { get; set; }
    public string Condition { get; set; }
    public ProductLocation Location { get; set; }
    public ProductQuantity AvailableQuantity { get; set; }
    public ProductQuantity StockQuantity { get; set; }
    public ProductPrice Price { get; set; }

    public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();

    public DateTime CreatedDateTime { get; set; }

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
        ProductId productId,
        string name,
        SKU sku,
        string condition,
        ProductLocation location,
        ProductQuantity availableQuantity,
        ProductQuantity stockQuantity,
        ProductPrice price,
        DateTime createdDateTime,
        DateTime updatedDateTime)
    {
        return new Product(
                productId,
                name,
                sku,
                condition,
                location,
                availableQuantity,
                stockQuantity,
                price,
                createdDateTime,
                updatedDateTime);
    }

    public static Product Create(
        string name,
        SKU sku,
        string condition,
        ProductLocation location,
        ProductQuantity availableQuantity,
        ProductQuantity stockQuantity,
        ProductPrice price)
    {
        return new Product(
                ProductId.CreateUnique(),
                name,
                sku,
                condition,
                location,
                availableQuantity,
                stockQuantity,
                price,
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