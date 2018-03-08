using Assets.Scripts.HexImpl;
using Assets.Scripts.Interfaces;
using GraphAlgorithms;
using System.Collections.Generic;
using UnityEngine;

class UnitController : MonoBehaviour
{
    [SerializeField] private GameObject tileMovementPrefab;
    [SerializeField] private GameObject pathArrowPrefab;

    private IUnit<HexNode> selectedUnit = null;
    private List<GameObject> highlightedTiles = new List<GameObject>();
    private List<GameObject> pathArrows = new List<GameObject>();
    private ITile<HexNode> hoverOver;

    private void Update()
    {
        RaycastHit hit;
        ITileEngine<HexNode> engine = HexEngine.Singleton;

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 200.0f, LayerMask.GetMask("Unit")))
            {
                Debug.Log("Unit selected");
                selectedUnit = hit.collider.gameObject.GetComponent<IUnit<HexNode>>();
                IEnumerable<IPathNode<HexNode>> reachable = engine.GetReachable(selectedUnit, selectedUnit.Tile);
                HighlightTiles(reachable);
            }
            else if (selectedUnit != null && !selectedUnit.PerformingAction() && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 200.0f, LayerMask.GetMask("Tile")))
            {
                HexCell cell = hit.collider.gameObject.GetComponent<HexCell>();

                Debug.Log("Hit cell at: " + cell.X + ", " + cell.Z);

                IEnumerable<IPathNode<HexNode>> path = engine.GetShortestPath(selectedUnit, selectedUnit.Tile, cell);
                selectedUnit.Move(path);
                ClearGameObjectList(highlightedTiles);
                ClearGameObjectList(pathArrows);
            }
        }

        if (Input.GetMouseButtonDown(1) && selectedUnit != null)
        {
            Debug.Log("Unit deselected");
            selectedUnit = null;
            ClearGameObjectList(highlightedTiles);
            ClearGameObjectList(pathArrows);
        }

        if (selectedUnit != null && !selectedUnit.PerformingAction() && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 200.0f, LayerMask.GetMask("Tile")))
        {
            ITile<HexNode> hoverNow = hit.collider.gameObject.GetComponent<ITile<HexNode>>();
            if (!selectedUnit.GetTerrainWalkability(hoverNow.Terrain).Passable)
                return;

            if (!hoverNow.Equals(hoverOver))
            {
                IEnumerable<IPathNode<HexNode>> path = engine.GetShortestPath(selectedUnit, selectedUnit.Tile, hoverNow);
                ClearGameObjectList(pathArrows);
                HighlightPath(path);
            }
        }
    }

    private void HighlightTiles(IEnumerable<IPathNode<HexNode>> reachable)
    {
        // Fix until selection of direction is implemented //
        Dictionary<ITile<HexNode>, bool> tiles = new Dictionary<ITile<HexNode>, bool>();

        foreach (IPathNode<HexNode> node in reachable)
        {
            ITile<HexNode> tile = node.GetNode().Tile;

            if (tiles.ContainsKey(tile))
                continue;

            tiles.Add(tile, true);
            Vector3 position = new Vector3(tile.WorldPosX, tile.WorldPosY + 0.05f, tile.WorldPosZ);

            GameObject highlight = Instantiate(tileMovementPrefab, position, tileMovementPrefab.transform.rotation, transform);
            highlightedTiles.Add(highlight);
        }
    }


    private void HighlightPath(IEnumerable<IPathNode<HexNode>> path)
    {
        Dictionary<ITile<HexNode>, bool> tiles = new Dictionary<ITile<HexNode>, bool>();

        IEnumerator<IPathNode<HexNode>> enumerator = path.GetEnumerator();
        enumerator.MoveNext(); // Skips first //
        tiles.Add(enumerator.Current.GetNode().Tile, true);

        while (enumerator.MoveNext())
        {
            IPathNode<HexNode> node = enumerator.Current;
            HexNode hexNode = node.GetNode();
            ITile<HexNode> tile = hexNode.Tile;

            if (tiles.ContainsKey(tile))
                continue;
            tiles.Add(tile, true);

            Vector3 position = new Vector3(tile.WorldPosX, tile.WorldPosY + 0.05f, tile.WorldPosZ);
            Quaternion rotation = Quaternion.Euler(90, hexNode.Direction.DirectionRotation() - 60, 0);

            GameObject highlight = Instantiate(pathArrowPrefab, position, rotation, transform);
            pathArrows.Add(highlight);
        }
    }

    private void ClearGameObjectList(List<GameObject> list)
    {
        foreach (GameObject obj in list)
        {
            Destroy(obj);
        }
        list.Clear();
    }
}
