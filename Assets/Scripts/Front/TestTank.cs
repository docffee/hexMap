using Assets.Scripts.HexImpl;
using GraphAlgorithms;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Assets.Scripts.Interfaces;

public class TestTank : UnityUnit
{
    public override void Move(IEnumerable<IPathNode<HexNode>> path, IReady controller)
    {
        StartCoroutine(MoveWaiter(path, controller));
    }

    public override bool PerformingAction()
    {
        return performingAction;
    }

    public override IWalkable GetTerrainWalkability(ITerrain terrain)
    {
        switch (terrain.Name)
        {
            case "Grass":
                return new Walkable(1, true);
            case "Forest":
                return new Walkable(2, true);
            case "Mountain":
                return new Walkable(0, false);
            case "Water":
                return new Walkable(0, false);
            case "Sand":
                return new Walkable(3, true);
            default:
                return new Walkable(0, false);
        }
    }

    public override float RotateCost
    {
        get
        {
            return 0.2f;
        }
    }

    public override int MaxActionPoints
    {
        get
        {
            return 2;
        }
    }

    public override int CurrentActionPoints
    {
        get
        {
            return 2;
        }
    }

    public override int Direction
    {
        get
        {
            return (int)orientation;
        }
    }

    public override float DisplacementY
    {
        get
        {
            return displacementY;
        }
    }

    public override ITile<HexNode> Tile
    {
        get
        {
            return tile;
        }

        set
        {
            tile = value;
        }
    }

    public override float MaxMovePoints
    {
        get { return maxMovePoints; }

        set { maxMovePoints = value; }
    }

    public override float CurrentMovePoints
    {
        get { return currentMovePoints; }

        set { currentMovePoints = value; }
    }

    public override bool Flying
    {
        get
        {
            return flying;
        }
    }
}
