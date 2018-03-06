using Assets.Scripts.HexImpl;
using GraphAlgorithms;
using System.Collections.Generic;

public interface IUnit<N> where N : INode<N>
{
    int MaxActionPoints { get; }
    int CurrentActionPoints { get; }
    float MaxMovePoints { get; set; }
    float CurrentMovePoints { get; set; }
    int Direction { get; }

    float RotateCost { get; }
    float GetTerrainModifier(ITerrain terrain);

    ITile<HexNode> Tile { get; set; }
    void Move(IEnumerable<IPathNode<N>> path);
    bool PerformingAction();
}