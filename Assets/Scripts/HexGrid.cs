using Assets.Scripts.HexImpl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour, IMapGenerator<HexNode>
{
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private HexCell hexCellPrefab;

    private HexCell[] cells;
    private HexMesh hexMesh;

    private void Start()
    {
        hexMesh = GetComponentInChildren<HexMesh>();
        cells = new HexCell[width * height];

        int i = 0;
        for (int z = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                CreateCell(x, z, i);
                i++;
            }
        }

        hexMesh.Initialize();
        hexMesh.Triangulate(cells);
    }

    private void CreateCell(int x, int z, int i)
    {
        Vector3 position;
        position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
        position.y = 0f;
        position.z = z * (HexMetrics.outerRadius * 1.5f);

        cells[i] = Instantiate(hexCellPrefab);
        cells[i].transform.SetParent(transform, false);
        cells[i].transform.localPosition = position;

        cells[i].Initialize(x, z);
        cells[i].GetComponentInChildren<TextMesh>().text = x + ", " + z;
        cells[i].color = GetRandomColor();

        cells[i].Elevation = Random.Range(0, 4);

        if (x > 0)
        {
            cells[i].SetNeighbourBiDirectional(cells[i - 1], HexDirection.W);
        }
        if (z > 0)
        {
            if ((z & 1) == 0)
            {
                cells[i].SetNeighbourBiDirectional(cells[i - width], HexDirection.SE);
                if (x > 0)
                {
                    cells[i].SetNeighbourBiDirectional(cells[i - width - 1], HexDirection.SW);
                }
            }
            else
            {
                cells[i].SetNeighbourBiDirectional(cells[i - width], HexDirection.SW);
                if (x < width - 1)
                {
                    cells[i].SetNeighbourBiDirectional(cells[i - width + 1], HexDirection.SE);
                }
            }
        }
    }

    private Color GetRandomColor()
    {
        int rnd = Random.Range(0, 4);
        
        switch (rnd)
        {
            case 0:
                return Color.blue;
            case 2:
                return Color.yellow;
            case 3:
                return Color.white;
            default:
                return Color.green;
        }
    }

    public ITile<HexNode>[] GenerateTiles(int sizeX, int sizeY)
    {
        return cells;
    }
}
