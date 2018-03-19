using Assets.Scripts.HexImpl;
using GraphAlgorithms;
using System.Collections.Generic;
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
}
