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
    [SerializeField] private GameObject abillityPanel;
    private UnitIconCamera unitIconCam = null;
    private EnemyUnitIconCamera enemyUnitIconCam = null;

    [Header ("Controller")]
    [SerializeField] private GameController gameController;

    private ITileControl<HexNode> hexControl = null;
    private Unit selectedUnit = null;
    private Unit enemyUnit = null;
    private IAction selectedAction;

    private List<GameObject> highlightedTiles = new List<GameObject>();
    private List<GameObject> highlightedPath = new List<GameObject>();
    private ITile hoverOver;
    private int hoverDirection;
    private Quaternion lastPathArrowRotation;
    private bool performingAction;

    private static UnitController singleton;

    public void Initialize(ITileControl<HexNode> hexControl)
    {
        enemyUnitPanel.gameObject.SetActive(false);
        unitPanel.gameObject.SetActive(false);
        this.hexControl = hexControl;
        unitIconCam = unitIconCamera.GetComponent<UnitIconCamera>();
        enemyUnitIconCam = enemyUnitIconCamera.GetComponent<EnemyUnitIconCamera>();
        singleton = this;
    }

    private void Update()   
    {
        if (performingAction && selectedUnit != null)
        {
            unitIconCam.CenterOn(selectedUnit.transform.position);
            return;
        }
        
        if(selectedUnit != null)    
            UnitTextUpdate();

        if (Input.GetMouseButtonDown(1) && selectedAction != null)
            StopAction();

        if (selectedAction != null && selectedAction.HasControl)
            return;

        RaycastHit tileHit;
        RaycastHit unitHit;

        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),
            out tileHit, 200.0f, LayerMask.GetMask("Tile"));
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),
            out unitHit, 200.0f, LayerMask.GetMask("Unit"));

        if (Input.GetMouseButtonDown(0))
            MouseClickEvent0(tileHit);
        else
            MouseOver(tileHit, unitHit);
    }

    private void MouseOver(RaycastHit tileHit, RaycastHit unitHit)
    {
        if(HoverUnit(unitHit))
            return;
        if(HoverTile(tileHit))
            return;
    }

    private bool HoverTile(RaycastHit tileHit)
    {
        ITile hoverNow = 
            (
                (tileHit.collider != null) ?
                tileHit.collider.gameObject.GetComponent<ITile>() :
                null
            );

        int dir = GetTileDirection(tileHit);

        if (selectedUnit != null && hoverNow != null && selectedUnit.IsTilePassable(hoverNow))
        {
            if (!selectedUnit.GetTerrainWalkability(hoverNow.Terrain).Passable)
                return false;

            if (!hoverNow.Equals(hoverOver) || hoverDirection != dir) // Calculates shortest path and shows it.
            {
                hoverDirection = dir;
                hoverOver = hoverNow;
                IEnumerable<IPathNode<HexNode>> path = hexControl.GetShortestPath(selectedUnit, selectedUnit.Tile, hoverNow, hoverDirection);
                ClearGameObjectList(highlightedPath);
                HighlightPath(path);

                int pathCount = highlightedPath.Count;
                if (dir == 6)
                    highlightedPath[pathCount - 1].transform.rotation = lastPathArrowRotation;
                else
                    highlightedPath[pathCount - 1].transform.rotation =
                        Quaternion.Euler(90, HexUtil.DirectionRotation((HexDirection)dir) - 90, 0);
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool HoverUnit(RaycastHit unitHit)
    {
        enemyUnit = 
            (
                (unitHit.collider != null) ?
                unitHit.collider.gameObject.GetComponent<Unit>() :
                null
            );
    

        if (unitHit.collider != null)
        {        
            ClearGameObjectList(highlightedPath);
            hoverOver = null;
            if (enemyUnit != selectedUnit)
            {
                enemyUnitIconCam.CenterOn(enemyUnit.transform.position);
                EnemyUnitTextUpdate();
                enemyUnitPanel.gameObject.SetActive(true);    
            }
            return true;    
        }
        else
        {
            enemyUnitPanel.gameObject.SetActive(false);
            return false;
        }
            
    }

    private void MouseClickEvent0(RaycastHit tileHit)
    {
        if (selectedUnit != null && !EventSystem.current.IsPointerOverGameObject() && tileHit.collider != null)
        {
            HexCell cell = tileHit.collider.gameObject.GetComponent<HexCell>();

            IEnumerable<IPathNode<HexNode>> path = hexControl.GetShortestPath(selectedUnit, selectedUnit.Tile, cell, hoverDirection);
            if (path != null)
            {
                performingAction = true;
                selectedUnit.Move(path, this);
                ClearGameObjectList(highlightedTiles);
                ClearGameObjectList(highlightedPath);
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
        GameObject currentTile = Instantiate(currentTilePrefab, currentTilePosition, currentTilePrefab.transform.rotation, transform);
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
            lastPathArrowRotation = rotation;
        }
    }

    private void ClearGameObjectList(List<GameObject> list)
    {
        foreach (GameObject obj in list)
            Destroy(obj);

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

        Button[] abillityButtons = abillityPanel.GetComponentsInChildren<Button>();
        IAction[] actions = selectedUnit.Actions;

        for (int i = 0; i < actions.Length; i++)
        {
            IAction action = actions[i];
            Button button = abillityButtons[i];

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => action.TakeControl());
            button.GetComponentInChildren<Text>().text = action.ActionName;
        }
    }
    
    private void UnitTextUpdate()
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

    private void EnemyUnitTextUpdate()
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

    private int GetTileDirection(RaycastHit tileHit)
    {
        int dir = 6;
        if (tileHit.collider != null)
        {
            Vector3 hit = tileHit.point;
            Vector3 tile = tileHit.collider.gameObject.transform.position;
            float dist = Vector3.Distance(tile, hit);
            const float centerR = 3f;


            if (dist <= centerR)
            {
                dir = 6;
            }
            else
            {
                Vector3 temp = hit - tile;
                Vector3 helperPos = new Vector3(0, 0, 1);
                float angle =
                    (
                        (temp.x > 0) ?
                        Vector3.Angle(helperPos, temp) :
                        180 + (Math.Abs(Vector3.Angle(helperPos, temp) - 180))
                    );
                dir = ((int)angle / 60);
            }
        }

        return dir;
    }
    
    public void SetAction(IAction action)
    {
        ClearGameObjectList(highlightedTiles);
        ClearGameObjectList(highlightedPath);
        selectedAction = action;
    }

    public void StopAction()
    {
        selectedAction.HasControl = false;
        selectedAction = null;
        IEnumerable<IPathNode<HexNode>> reachable = hexControl.GetReachable(selectedUnit, selectedUnit.Tile);
        HighlightTiles(reachable);
    }

    public static UnitController Singleton
    {
        get
        {
            return singleton;
        }
    }
}
