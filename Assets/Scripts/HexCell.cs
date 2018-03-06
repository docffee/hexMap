using System;
using Assets.Scripts.HexImpl;
using UnityEngine;

public class HexCell : MonoBehaviour, ITile<HexNode>
{
    public Color color;

    private int x, z;
    private HexCell[] neighbours;
    private int elevation;

    private ITerrain terrain;
    private IUnit<HexNode> unit;

    public void Initialize(int x, int z, ITerrain terrain)
    {
        neighbours = new HexCell[6];
        this.x = x;
        this.z = z;
        this.terrain = terrain;
    }

    public void SetNeighbourBiDirectional(HexCell cell, HexDirection dir)
    {
        neighbours[(int)dir] = cell;
        cell.neighbours[(int)dir.OppositeDirection()] = this;
    }

    public HexCell GetNeighbour(int index)
    {
        return neighbours[index];
    }

    public HexCell GetNeighbour(HexDirection direction)
    {
        return neighbours[(int)direction];
    }

    public HexSlopeType GetNeighbourSlopeType(HexDirection direction)
    {
        return HexMetrics.GetHexSlopeType(elevation, neighbours[(int)direction].elevation);
    }

    public int Elevation
    {
        get
        {
            return elevation;
        }

        set
        {
            elevation = value;
            Vector3 position = transform.localPosition;
            position.y = value * HexMetrics.elevationStep;
            transform.localPosition = position;
        }
    }

    public override string ToString()
    {
        return x + ", " + z;
    }

    public int X
    {
        get { return x; }
    }

    public int Z
    {
        get { return z; }
    }

    public int Y
    {
        get { return elevation; }
    }

    public ITerrain Terrain
    {
        get { return terrain; }
    }

    public IUnit<HexNode> UnitOnTile
    {
        get { return unit; }
    }

    public float WorldPosX
    {
        get { return transform.position.x; }
    }

    public float WorldPosY
    {
        get { return transform.position.y; }
    }

    public float WorldPosZ
    {
        get { return transform.position.z; }
    }

    IUnit<HexNode> ITile<HexNode>.UnitOnTile
    {
        get
        {
            return unit;
        }

        set
        {
            unit = value;
        }
    }
}
