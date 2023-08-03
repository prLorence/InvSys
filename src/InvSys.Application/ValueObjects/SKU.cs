using InvSys.Application.Common;

namespace InvSys.Application.ValueObjects;

public sealed class SKU : ValueObject
{
    public string Value { get; }

#pragma warning disable CS8618
    private SKU() { }
#pragma warning restore CS8618

    private SKU(string value)
    {
        Value = value;
    }

    public static SKU Create(string value)
    {
        return new(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}