using Assets.Scripts.HexImpl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour, IMapGenerator<HexNode>
{
    [SerializeField] private int sizeX;
    [SerializeField] private int sizeZ;
    [SerializeField] private HexCell hexCellPrefab;

    private HexCell[] cells;
    private HexMesh hexMesh;

    private void Start()
    {
        hexMesh = GetComponentInChildren<HexMesh>();
        //cells = new HexCell[sizeX * sizeZ];

        //int i = 0;
        //for (int z = 0; z < sizeZ; z++)
        //{
        //    for (int x = 0; x < sizeX; x++)
        //    {
        //        CreateCell(x, z, i, Assets.Scripts.HexImpl.Terrain.GetTerrainFromChar('F'));
        //        i++;
        //    }
        //}

        BreindalenMap map = new BreindalenMap();
        CreateMeshFromCells(map.GenerateTiles(40, 20), 40, 20);

        hexMesh.Initialize();
        hexMesh.Triangulate(cells);
    }

    public void CreateMeshFromCells(ITile<HexNode>[] tiles, int sizeX, int sizeY)
    {
        cells = new HexCell[sizeX * sizeY];

        int i = 0;
        for (int z = 0; z < sizeY; z++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                ITile<HexNode> tile = tiles[i];
                CreateCell(x, z, i, tile.Terrain);
                i++;
            }
        }
    }

    private void CreateCell(int x, int z, int i, ITerrain terrain)
    {
        Vector3 position;
        position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
        position.y = 0f;
        position.z = z * (HexMetrics.outerRadius * 1.5f);

        cells[i] = Instantiate(hexCellPrefab);
        cells[i].transform.SetParent(transform, false);
        cells[i].transform.localPosition = position;

        cells[i].Initialize(x, z, terrain);
        cells[i].GetComponentInChildren<TextMesh>().text = x + ", " + z;

        //cells[i].Elevation = Random.Range(0, 4);
        cells[i].color = GetColorFromTerrain(terrain);
        
        if (x > 0)
        {
            cells[i].SetNeighbourBiDirectional(cells[i - 1], HexDirection.W);
        }
        if (z > 0)
        {
            if ((z & 1) == 0)
            {
                cells[i].SetNeighbourBiDirectional(cells[i - sizeX], HexDirection.SE);
                if (x > 0)
                {
                    cells[i].SetNeighbourBiDirectional(cells[i - sizeX - 1], HexDirection.SW);
                }
            }
            else
            {
                cells[i].SetNeighbourBiDirectional(cells[i - sizeX], HexDirection.SW);
                if (x < sizeX - 1)
                {
                    cells[i].SetNeighbourBiDirectional(cells[i - sizeX + 1], HexDirection.SE);
                }
            }
        }
    }

    private Color GetElevationColor(int elevation)
    {
        switch (elevation)
        {
            case 0:
                return Color.blue;
            case 1:
                return Color.yellow;
            case 2:
                return Color.green;
            case 3:
                return Color.grey;
            default:
                return Color.white;
        }
    }

    private Color GetRandomColor()
    {
        int rng = Random.Range(0, 4);
        switch (rng)
        {
            case 0:
                return Color.blue;
            case 1:
                return Color.yellow;
            case 2:
                return Color.green;
            case 3:
                return Color.grey;
            default:
                return Color.white;
        }
    }

    private Color GetColorFromTerrain(ITerrain terrain)
    {
        switch (terrain.Name)
        {
            case "Grass":
                return Color.green;
            case "Sand":
                return Color.yellow;
            case "Mountain":
                return Color.grey;
            case "Water":
                return Color.blue;
            case "Forest":
                return new Color32(0, 151, 51, 255);
            default:
                return Color.white;
        }
    }

    public ITile<HexNode>[] GenerateTiles(int sizeX, int sizeY)
    {
        return cells;
    }

    public int SizeX
    {
        get { return sizeX; }
    }

    public int SizeZ
    {
        get { return sizeZ; }
    }
}
