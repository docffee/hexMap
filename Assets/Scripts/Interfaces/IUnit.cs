﻿using Assets.Scripts.HexImpl;
using Assets.Scripts.Interfaces;
using GraphAlgorithms;
using System.Collections.Generic;

public interface IUnit<N> where N : INode<N>
{
    int MaxActionPoints { get; }
    int CurrentActionPoints { get; }
    float MaxMovePoints { get; set; }
    float CurrentMovePoints { get; set; }
    int Direction { get; }
    bool Flying { get; }

    float RotateCost { get; }
    IWalkable GetTerrainWalkability(ITerrain terrain);

    ITile<HexNode> Tile { get; set; }
    void Move(IEnumerable<IPathNode<N>> path, IReady controller);
    bool PerformingAction();
}