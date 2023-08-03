using InvSys.Application.Common;

namespace InvSys.Application.ValueObjects;

public sealed class ProductQuantity : ValueObject
{
    public int Value { get; }

    private ProductQuantity() { }

    private ProductQuantity(int value)
    {
        Value = value;
    }

    public static ProductQuantity Create(int value)
    {
        return value < 0 ? new(0) : new(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}