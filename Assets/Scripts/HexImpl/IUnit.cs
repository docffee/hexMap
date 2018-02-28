using Assets.Scripts.HexImpl;
using GraphAlgorithms;
using System.Collections.Generic;

public interface IUnit<N> where N : INode<N>
{
    int MaxActionPoints { get; }
    int CurrentActionPoints { get; }
    int Direction { get; }

    ITile<HexNode> Tile { get; set; }
    void Move(IEnumerable<IPathNode<N>> path);
    bool PerformingAction();
}