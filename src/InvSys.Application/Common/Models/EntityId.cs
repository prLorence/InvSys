namespace InvSys.Application.Common.Models;

public abstract class EntityId<TId> : ValueObject
{
    public abstract TId Value { get; protected set; }
}