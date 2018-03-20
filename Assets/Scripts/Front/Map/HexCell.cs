using System;
using Assets.Scripts.HexImpl;
using UnityEngine;

public class HexCell : MonoBehaviour, ITile
{
    public Color color;

    private int x, z;
    private HexCell[] neighbours;
    private int elevation;

    private ITerrain terrain;
    private IUnit unit;
    private IUnit airUnit;

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

    public override bool Equals(object obj)
    {
        var cell = obj as HexCell;
        if (cell == null)
            return false;

        return x == cell.x && z == cell.z;
    }

    public override int GetHashCode()
    {
        var hashCode = -300798877;
        hashCode = hashCode * -1521134295 + x.GetHashCode();
        hashCode = hashCode * -1521134295 + z.GetHashCode();
        return hashCode;
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

    public IUnit UnitOnTile
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

    IUnit ITile.UnitOnTile
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

    public IUnit AirUnitOnTile
    {
        get
        {
            return airUnit;
        }

        set
        {
            airUnit = value;
        }
    }
}
