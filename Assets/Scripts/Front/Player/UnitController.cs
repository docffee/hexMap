﻿using Assets.Scripts.HexImpl;
using Assets.Scripts.Interfaces;
using GraphAlgorithms;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class UnitController : MonoBehaviour, IReady
{
    [SerializeField] private GameObject tileMovementPrefab;
    [SerializeField] private GameObject pathArrowPrefab;
    [SerializeField] private GameObject unitTypeText;
    [SerializeField] private GameObject movementText;
    [SerializeField] private GameObject unitIcon;
    [SerializeField] protected GameObject unitPanel; 
    private UnityUnit selectedUnit = null;
    private List<GameObject> highlightedTiles = new List<GameObject>();
    private List<GameObject> pathArrows = new List<GameObject>();
    private ITile hoverOver;
    private bool performingAction;

    private void Start()
    {
        unitPanel.gameObject.SetActive(false);    
    }

    private void Update()   
    {
        if (performingAction)
            return;

        if(selectedUnit != null)
        {
            movementText.GetComponent<Text>().text = 
                "Move points:	" + selectedUnit.CurrentActionPoints + "/" + selectedUnit.MaxActionPoints;
        }

        RaycastHit hit;
        ITileControl<HexNode> hexControl = HexControl.Singleton;

        if (Input.GetMouseButtonDown(0))
        {
            if (selectedUnit != null && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 200.0f, LayerMask.GetMask("Unit"))
                && hit.collider.GetComponent<UnityUnit>().Controller.Team != selectedUnit.Controller.Team)
            {
                if (selectedUnit.CurrentActionPoints >= selectedUnit.AttackActionPointCost)
                {
                    UnityUnit other = hit.collider.gameObject.GetComponent<UnityUnit>();

                    // Bad solution, change when there is time //
                    int dist = (int)new HexHeuristic().MinDist(new HexNode(0, selectedUnit.Tile, null), new HexNode(0, other.Tile, null));
                    if (dist <= selectedUnit.Range)
                    {
                        IFireFight fireFight = new FireFight();
                        fireFight.Fight(selectedUnit, other);
                        Debug.Log("Fighting!");
                    }
                }
            }
            else if (selectedUnit != null && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 200.0f, LayerMask.GetMask("Tile")))
            {
                HexCell cell = hit.collider.gameObject.GetComponent<HexCell>();

                IEnumerable<IPathNode<HexNode>> path = hexControl.GetShortestPath(selectedUnit, selectedUnit.Tile, cell);
                if (path != null)
                {
                    performingAction = true;
                    selectedUnit.Move(path, this);
                    ClearGameObjectList(highlightedTiles);
                    ClearGameObjectList(pathArrows);
                }
            }
            else if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 200.0f, LayerMask.GetMask("Unit")))
            {
                Debug.Log("Unit selected");
                selectedUnit = hit.collider.gameObject.GetComponent<UnityUnit>();
                unitTypeText.GetComponent<Text>().text = selectedUnit.UnitName;

                unitPanel.gameObject.SetActive(true);
                IEnumerable<IPathNode<HexNode>> reachable = hexControl.GetReachable(selectedUnit, selectedUnit.Tile);
                unitIcon.GetComponent<Image>().sprite = selectedUnit.Icon;
                ClearGameObjectList(highlightedTiles);
                HighlightTiles(reachable);
            }
        }

        if (Input.GetMouseButtonDown(1) && selectedUnit != null)
        {
            Debug.Log("Unit deselected");
            unitPanel.gameObject.SetActive(false);
            selectedUnit = null;
            ClearGameObjectList(highlightedTiles);
            ClearGameObjectList(pathArrows);
        }

        if (selectedUnit != null && !selectedUnit.PerformingAction() && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 200.0f, LayerMask.GetMask("Tile")))
        {
            ITile hoverNow = hit.collider.gameObject.GetComponent<ITile>();

            if (hoverNow.UnitOnTile != null)
                return;
            if (!selectedUnit.GetTerrainWalkability(hoverNow.Terrain).Passable)
                return;

            if (!hoverNow.Equals(hoverOver))
            {
                hoverOver = hoverNow;
                IEnumerable<IPathNode<HexNode>> path = hexControl.GetShortestPath(selectedUnit, selectedUnit.Tile, hoverNow);
                ClearGameObjectList(pathArrows);
                HighlightPath(path);
            }
        }
    }

    private void HighlightTiles(IEnumerable<IPathNode<HexNode>> reachable)
    {
        if (highlightedTiles.Count > 0)
            return;

        // Fix until selection of direction is implemented //
        Dictionary<ITile, bool> tiles = new Dictionary<ITile, bool>();

        foreach (IPathNode<HexNode> node in reachable)
        {
            ITile tile = node.GetNode().Tile;

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
        if (path == null)
            return;

        Dictionary<ITile, bool> tiles = new Dictionary<ITile, bool>();

        IEnumerator<IPathNode<HexNode>> enumerator = path.GetEnumerator();
        enumerator.MoveNext(); // Skips first //
        tiles.Add(enumerator.Current.GetNode().Tile, true);

        while (enumerator.MoveNext())
        {
            IPathNode<HexNode> node = enumerator.Current;
            HexNode hexNode = node.GetNode();
            ITile tile = hexNode.Tile;

            if (tiles.ContainsKey(tile))
                continue;
            tiles.Add(tile, true);

            Vector3 position = new Vector3(tile.WorldPosX, tile.WorldPosY + 0.05f, tile.WorldPosZ);
            Quaternion rotation = Quaternion.Euler(90, hexNode.Direction.DirectionRotation() - 90, 0);

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

    public void Ready()
    {
        IEnumerable<IPathNode<HexNode>> reachable = HexControl.Singleton.GetReachable(selectedUnit, selectedUnit.Tile);
        performingAction = false;
        HighlightTiles(reachable);
    }
}
