using System;

public interface IPlayer : IEquatable<IPlayer>
{
    int Id { get; }
    int Money { get; set; }
    int Team { get; }
}