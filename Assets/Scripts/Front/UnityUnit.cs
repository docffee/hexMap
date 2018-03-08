using Assets.Scripts.HexImpl;
using GraphAlgorithms;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnityUnit : MonoBehaviour, IUnit<HexNode>
{
    public abstract int MaxActionPoints { get; }
    public abstract int CurrentActionPoints { get; }
    public abstract float MaxMovePoints { get; set; }
    public abstract float CurrentMovePoints { get; set; }
    public abstract int Direction { get; }
    public abstract float RotateCost { get; }
    public abstract ITile<HexNode> Tile { get; set; }

    public abstract IWalkable GetTerrainWalkability(ITerrain terrain);

    public abstract void Move(IEnumerable<IPathNode<HexNode>> path);
    public abstract bool PerformingAction();

    public abstract float DisplacementY { get; }
}
