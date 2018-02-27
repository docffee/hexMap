using UnityEngine;

public class HexCell : MonoBehaviour
{
    public Color color;

    private int x, z;
    private HexCell[] neighbours;
    private int elevation;

    public void Initialize(int x, int z)
    {
        neighbours = new HexCell[6];
        this.x = x;
        this.z = z;
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

    public int X
    {
        get { return x; }
    }

    public int Z
    {
        get { return z; }
    }
}
