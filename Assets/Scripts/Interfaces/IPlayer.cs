using System;

public interface IPlayer : IEquatable<IPlayer>
{
    int Id { get; }
    int Team { get; }
    int GetResource(ResourceType type);
    void SetResource(ResourceType type, int amount);
}