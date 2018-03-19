using System;

public interface ITerrain : IEquatable<ITerrain>
{
    string Name { get; }
}