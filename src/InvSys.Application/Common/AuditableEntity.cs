namespace InvSys.Application.Common;

public abstract class AuditableEntity<TId> : IEquatable<AuditableEntity<TId>>
    where TId : notnull
{
    public TId Id { get; private set; }

    protected AuditableEntity(TId id)
    {
        Id = id;
    }

#pragma warning disable CS8618
    protected AuditableEntity()
    {
    }
#pragma warning restore CS8618

    public override bool Equals(object? obj)
    {
        return obj is AuditableEntity<TId> entity && Id.Equals(entity.Id);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public bool Equals(AuditableEntity<TId>? other)
    {
        return Equals((object?)other);
    }

    public static bool operator ==(AuditableEntity<TId> left, AuditableEntity<TId> right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(AuditableEntity<TId> left, AuditableEntity<TId> right)
    {
        return !Equals(left, right);
    }

}