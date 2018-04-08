using Assets.Scripts.HexImpl;
using Assets.Scripts.Interfaces;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Front.Actions;

public class UnitController : MonoBehaviour
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

    private int hoverDirection;
    private Quaternion lastPathArrowRotation;

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
        //if (performingAction && selectedUnit != null)
        //{
        //    unitIconCam.CenterOn(selectedUnit.transform.position);
        //    return;
        //}
        
        if(selectedUnit != null)    
            UnitTextUpdate();

        if (Input.GetMouseButtonDown(1) && selectedAction != null && !selectedAction.ActionName.Equals("Move"))
            StopAction();

        if (selectedAction != null && selectedAction.HasControl)
            return;

        RaycastHit tileHit;
        RaycastHit unitHit;

        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),
            out tileHit, 200.0f, LayerMask.GetMask("Tile"));
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),
            out unitHit, 200.0f, LayerMask.GetMask("Unit"));

        MouseOver(tileHit, unitHit);
    }

    private void MouseOver(RaycastHit tileHit, RaycastHit unitHit)
    {
        if(HoverUnit(unitHit))
            return;
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

    public void SetSelectedUnit(Unit unit)
    {
        if (selectedAction != null)
            StopAction();

        selectedUnit = unit;
        unitIconCam.CenterOn(selectedUnit.transform.position);
        unitPanel.gameObject.SetActive(true);

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

        selectedAction = new MoveAction(unit, hexControl);
        selectedAction.TakeControl();
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
    
    public void SetAction(IAction action)
    {
        if (selectedAction != null)
            StopAction();

        selectedAction = action;
    }

    private void StopAction()
    {
        selectedAction.HasControl = false;
        selectedAction = null;
    }

    public void ResetAction()
    {
        if (selectedAction != null)
            StopAction();

        selectedAction = new MoveAction(selectedUnit, hexControl);
        selectedAction.TakeControl();
    }

    public static UnitController Singleton
    {
        get
        {
            return singleton;
        }
    }
}
