﻿namespace Domain.Common.Abstractions;
public abstract class Entity<T> : Entity, IEntity<T>
{
    public T Id { get; protected set; } = default!;

}

public abstract class Entity : IEntity
{
    public DateTimeOffset DateCreated { get; private set; } = DateTimeOffset.UtcNow;
}
