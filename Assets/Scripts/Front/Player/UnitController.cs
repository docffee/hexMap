using Assets.Scripts.HexImpl;
using Assets.Scripts.Interfaces;
using GraphAlgorithms;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitController : MonoBehaviour, IReady
{
    [SerializeField] private GameObject tileMovementPrefab;
    [SerializeField] private GameObject pathArrowPrefab;
    [SerializeField] private GameObject currentTilePrefab;
    
    [Header ("UI elements")]
    [SerializeField] private GameObject unitTypeText;
    [SerializeField] private GameObject UnitMovementText;
    [SerializeField] private GameObject UnitDamageText;
    [SerializeField] private GameObject UnitHealthText;
    [SerializeField] private GameObject UnitActionCostText;
    [SerializeField] private GameObject UnitRangeText;
    [SerializeField] private GameObject UnitTurnCostText;
    [SerializeField] private GameObject enemyUnitTypeText;
    [SerializeField] private GameObject enemyUnitMovementText;
    [SerializeField] private GameObject enemyUnitDamageText;
    [SerializeField] private GameObject enemyUnitHealthText;
    [SerializeField] private GameObject enemyUnitActionCostText;
    [SerializeField] private GameObject enemyUnitRangeText;
    [SerializeField] private GameObject enemyUnitTurnCostText;
    [SerializeField] private GameObject unitIcon;
    [SerializeField] private GameObject unitPanel;
    [SerializeField] private GameObject enemyUnitPanel;
    [SerializeField] private GameObject unitIconCamera;
    [SerializeField] private GameObject enemyUnitIconCamera;
    private UnitIconCamera unitIconCam = null;
    private EnemyUnitIconCamera enemyUnitIconCam = null;

    [Header ("Controller")]
    [SerializeField] private GameController gameController;
    private ITileControl<HexNode> hexControl;
    private Unit selectedUnit = null;
    private Unit enemyUnit = null;
    private List<GameObject> highlightedTiles = new List<GameObject>();
    private List<GameObject> highlightedPath = new List<GameObject>();
    private GameObject currentTile;
    private ITile hoverOver;
    private bool performingAction;

    public void Initialize(ITileControl<HexNode> hexControl)
    {
        enemyUnitPanel.gameObject.SetActive(false);
        unitPanel.gameObject.SetActive(false);
        this.hexControl = hexControl;
        unitIconCam = unitIconCamera.GetComponent<UnitIconCamera>();
        enemyUnitIconCam = enemyUnitIconCamera.GetComponent<EnemyUnitIconCamera>();
    }

    private void Update()   
    {
        if (performingAction && selectedUnit != null){
            unitIconCam.CenterOn(selectedUnit.transform.position);

            return;
        }
            
        if(selectedUnit != null)
        {    
            unitTextUpdate();
        }

        RaycastHit hit;

        if (Input.GetMouseButtonDown(0))
        {
            if (selectedUnit != null && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 200.0f, LayerMask.GetMask("Unit"))
                && hit.collider.GetComponent<Unit>().Controller.Team != selectedUnit.Controller.Team)
            {
                if (selectedUnit.CurrentActionPoints >= selectedUnit.AttackActionPointCost)
                {
                    Unit other = hit.collider.gameObject.GetComponent<Unit>();

                    int dist = HexHeuristic.MinDistTile(selectedUnit.Tile, other.Tile);
                    if (dist <= selectedUnit.Range)
                    {
                        IFireFight fireFight = new FireFight();
                        fireFight.Fight(selectedUnit, other);
                        ClearGameObjectList(highlightedTiles);
                        IEnumerable<IPathNode<HexNode>> path = hexControl.GetReachable(selectedUnit, selectedUnit.Tile);
                        HighlightTiles(path);
                        Debug.Log("Fighting!");
                    }
                }
            }
            if (selectedUnit != null && !EventSystem.current.IsPointerOverGameObject()
                && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 200.0f, LayerMask.GetMask("Tile")))
            {
                HexCell cell = hit.collider.gameObject.GetComponent<HexCell>();

                IEnumerable<IPathNode<HexNode>> path = hexControl.GetShortestPath(selectedUnit, selectedUnit.Tile, cell);
                if (path != null)
                {
                    performingAction = true;
                    selectedUnit.Move(path, this);
                    ClearGameObjectList(highlightedTiles);
                    ClearGameObjectList(highlightedPath);
                }
            }
            //else if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 200.0f, LayerMask.GetMask("Unit")))
            //{
            //    Unit current = hit.collider.gameObject.GetComponent<Unit>();
            //    if (gameController.CurrentUnit.Equals(current))
            //    {
            //        selectedUnit = current;
            //        unitTypeText.GetComponent<Text>().text = selectedUnit.UnitName;

            //        unitPanel.gameObject.SetActive(true);
            //        IEnumerable<IPathNode<HexNode>> reachable = hexControl.GetReachable(selectedUnit, selectedUnit.Tile);
            //        unitIcon.GetComponent<Image>().sprite = selectedUnit.Icon;
            //        ClearGameObjectList(highlightedTiles);
            //        ClearGameObjectList(highlightedPath);
            //        HighlightTiles(reachable);
            //    }
            //}
        }

        //if (Input.GetMouseButtonDown(1) && selectedUnit != null)
        //{
        //    unitPanel.gameObject.SetActive(false);
        //    selectedUnit = null;
        //    ClearGameObjectList(highlightedTiles);
        //    ClearGameObjectList(highlightedPath);
        //}

        if (selectedUnit != null && !selectedUnit.PerformingAction() && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 200.0f, LayerMask.GetMask("Tile")))
        {
            ITile hoverNow = hit.collider.gameObject.GetComponent<ITile>();
            
            if ((hoverNow.UnitOnTile != null || hoverNow.AirUnitOnTile != null) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 200.0f, LayerMask.GetMask("Unit")))
            {        
                enemyUnit = hit.collider.gameObject.GetComponent<Unit>();
                
                if (enemyUnit != selectedUnit)
                {
                    enemyUnitIconCam.CenterOn(enemyUnit.transform.position);
                    enemyUnitTextUpdate();
                    enemyUnitPanel.gameObject.SetActive(true);    
                }
                
            }
            else{
                enemyUnitPanel.gameObject.SetActive(false);
            }
            if (!selectedUnit.GetTerrainWalkability(hoverNow.Terrain).Passable)
                return;

            if (!hoverNow.Equals(hoverOver))
            {
                hoverOver = hoverNow;
                IEnumerable<IPathNode<HexNode>> path = hexControl.GetShortestPath(selectedUnit, selectedUnit.Tile, hoverNow);
                ClearGameObjectList(highlightedPath);
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
            Vector3 position = new Vector3(tile.PosX, tile.PosY + 0.05f, tile.PosZ);

            GameObject highlight = Instantiate(tileMovementPrefab, position, tileMovementPrefab.transform.rotation, transform);    
            highlightedTiles.Add(highlight);
        }
        
        Vector3 currentTilePosition = new Vector3(selectedUnit.Tile.PosX, selectedUnit.Tile.PosY+0.10f, selectedUnit.Tile.PosZ);
        currentTile = Instantiate(currentTilePrefab, currentTilePosition, currentTilePrefab.transform.rotation, transform);
        highlightedTiles.Add(currentTile);
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

            Vector3 position = new Vector3(tile.PosX, tile.PosY + 0.05f, tile.PosZ);
            Quaternion rotation = Quaternion.Euler(90, hexNode.Direction.DirectionRotation() - 90, 0);

            GameObject highlight = Instantiate(pathArrowPrefab, position, rotation, transform);
            highlightedPath.Add(highlight);
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
        IEnumerable<IPathNode<HexNode>> reachable = hexControl.GetReachable(selectedUnit, selectedUnit.Tile);
        performingAction = false;
        HighlightTiles(reachable);
    }

    public void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
        unitIconCam.CenterOn(selectedUnit.transform.position);
        unitPanel.gameObject.SetActive(true);
        IEnumerable<IPathNode<HexNode>> reachable = hexControl.GetReachable(selectedUnit, selectedUnit.Tile);
        ClearGameObjectList(highlightedTiles);
        ClearGameObjectList(highlightedPath);
        HighlightTiles(reachable);
    }
    
    private void unitTextUpdate()
    {
        unitTypeText.GetComponent<Text>().text =
            selectedUnit.UnitName;
        UnitMovementText.GetComponent<Text>().text =
            "Action points:\t\t" + Math.Round(selectedUnit.CurrentActionPoints, 1) + "/" + selectedUnit.MaxActionPoints;
        UnitDamageText.GetComponent<Text>().text =
            "Damage:\t\t\t\t" + selectedUnit.Damage;
        UnitHealthText.GetComponent<Text>().text =
            "Health:\t\t\t\t\t" + selectedUnit.CurrentHealth + "/" + selectedUnit.MaxHealth;
        UnitActionCostText.GetComponent<Text>().text =
            "Attack cost:\t\t\t" + selectedUnit.AttackActionPointCost;
        UnitRangeText.GetComponent<Text>().text =
            "Attack Range:\t\t" + selectedUnit.Range;
        UnitTurnCostText.GetComponent<Text>().text =
            "Turn cost:\t\t\t\t" + selectedUnit.RotateCost;
    }

    private void enemyUnitTextUpdate()
    {
        enemyUnitTypeText.GetComponent<Text>().text =
            enemyUnit.UnitName;
        enemyUnitMovementText.GetComponent<Text>().text =
            "Action points:\t\t" + Math.Round(enemyUnit.CurrentActionPoints, 1) + "/" + enemyUnit.MaxActionPoints;
        enemyUnitDamageText.GetComponent<Text>().text =
            "Damage:\t\t\t\t" + enemyUnit.Damage;
        enemyUnitHealthText.GetComponent<Text>().text =
            "Health:\t\t\t\t\t" + enemyUnit.CurrentHealth + "/" + enemyUnit.MaxHealth;
        enemyUnitActionCostText.GetComponent<Text>().text =
            "Attack cost:\t\t\t" + enemyUnit.AttackActionPointCost;
        enemyUnitRangeText.GetComponent<Text>().text =
            "Attack Range:\t\t" + enemyUnit.Range;
        enemyUnitTurnCostText.GetComponent<Text>().text =
            "Turn cost:\t\t\t\t" + enemyUnit.RotateCost;
    }

}
