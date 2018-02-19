using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCell : MonoBehaviour
{
    public Color color;

    public HexNode hex;
    private HexCell[] neighbours;
    private int elevation;

    public void Initialize(int x, int z, List<INode> test)
    {
        neighbours = new HexCell[6];
        hex = new HexNode(1, x, z, test);
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
}
