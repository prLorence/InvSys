using InvSys.Application.Common;

namespace InvSys.Application.ValueObjects;

public sealed class ProductPrice : ValueObject
{
    public double Value { get; }

    private ProductPrice() { }

    private ProductPrice(double value)
    {
        Value = value;
    }

    public static ProductPrice Create(double value)
    {
        return value < 0 ? new(0) : new(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}