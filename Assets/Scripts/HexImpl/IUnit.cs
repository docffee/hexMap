using GraphAlgorithms;
using System.Collections.Generic;

public interface IUnit<N> where N : INode<N>
{
    int MaxActionPoints { get; }
    int CurrentActionPoints { get; }
    int Direction { get; }

    void Move(IEnumerable<IEdge<N>> path);
}