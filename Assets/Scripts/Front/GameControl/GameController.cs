using Assets.Scripts.HexImpl;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///     Controls the game flow, from start to end as well
///     as ending turns.
/// </summary>
public class GameController : MonoBehaviour
{
    [SerializeField] private UnitController unitController;
    [SerializeField] private Player playerPrefab;
    [SerializeField] private int numberOfPlayers;
    [SerializeField] private int startMoney;
    [SerializeField] private GameObject unitCamera;
    [SerializeField] private GameObject[] unitTypeTexts;
    [SerializeField] private GameObject[] unitPlayerTexts;
    [SerializeField] private GameObject[] unitQueuePanels;
    [SerializeField] private Material[] playerColors;
    [SerializeField] private GameObject[] unitPanels;
    [SerializeField] private GameObject miniMapPanel;
    [SerializeField] private GameObject queuePanel;
    [SerializeField] private GameObject commandPanel;
    [SerializeField] private RectTransform uiControlPanel;
    [SerializeField] private GameObject[] economyPanelTexts;

    [Header ("This is only temporary")]
    [SerializeField] private Unit[] allUnitPrefabs;

    HexControl hexControl;
    private List<IPlayer> players;
    private List<Unit> units;
    private Unit currentUnit;
    private Unit thisUnit;
    private int turnPointer;
    private List<Unit> unitsSorted;
    private Vector3 oldControlPanelPosition;
    private int unitQueueLimit = 8;

    private void Start()
    {
        oldControlPanelPosition = uiControlPanel.localPosition;
        HexGrid grid = FindObjectOfType<HexGrid>();
        hexControl = new HexControl(grid.SizeX, grid.SizeZ, grid);
        players = new List<IPlayer>();
        units = new List<Unit>();

        unitController.Initialize(hexControl);

        for (int i = 0; i < numberOfPlayers; i++)
        {
            Player player = Instantiate(playerPrefab);
            player.Initialize(i, i, startMoney);
            players.Add(player);
        }

        // Test stuff, delete when done //
        ITile tile = hexControl.GetTile(17, 12);
        ITile tile2 = hexControl.GetTile(19, 12);
        ITile tile3 = hexControl.GetTile(23, 12);
        ITile tile4 = hexControl.GetTile(21, 12);
        ITile tile5 = hexControl.GetTile(17, 13);
        ITile tile6 = hexControl.GetTile(19, 13);
        ITile tile7 = hexControl.GetTile(23, 13);
        ITile tile8 = hexControl.GetTile(21, 13);
        ITile tile9= hexControl.GetTile(17, 10);
        ITile tile10 = hexControl.GetTile(19, 14);
        ITile tile11 = hexControl.GetTile(23, 14);
        ITile tile12 = hexControl.GetTile(21, 14);

        Unit unit1 = SpawnTroop(allUnitPrefabs[0], players[0], tile, HexDirection.E);
        Unit unit2 = SpawnTroop(allUnitPrefabs[1], players[0], tile2, HexDirection.E);
        Unit unit3 = SpawnTroop(allUnitPrefabs[2], players[1], tile3, HexDirection.W);
        Unit unit4 = SpawnTroop(allUnitPrefabs[3], players[1], tile4, HexDirection.W);
        Unit unit5 = SpawnTroop(allUnitPrefabs[0], players[0], tile5, HexDirection.E);
        Unit unit6 = SpawnTroop(allUnitPrefabs[1], players[0], tile6, HexDirection.E);
        Unit unit7 = SpawnTroop(allUnitPrefabs[2], players[1], tile7, HexDirection.W);
        Unit unit8 = SpawnTroop(allUnitPrefabs[3], players[1], tile8, HexDirection.W);
        Unit unit9 = SpawnTroop(allUnitPrefabs[0], players[0], tile9, HexDirection.E);
        Unit unit10 = SpawnTroop(allUnitPrefabs[1], players[0], tile10, HexDirection.E);
        Unit unit11 = SpawnTroop(allUnitPrefabs[2], players[1], tile11, HexDirection.W);
        Unit unit12 = SpawnTroop(allUnitPrefabs[3], players[1], tile12, HexDirection.W);

        unit1.SetUnitColorMaterial(playerColors[0]);
        unit2.SetUnitColorMaterial(playerColors[0]);
        unit3.SetUnitColorMaterial(playerColors[1]);
        unit4.SetUnitColorMaterial(playerColors[1]);
        unit5.SetUnitColorMaterial(playerColors[0]);
        unit6.SetUnitColorMaterial(playerColors[0]);
        unit7.SetUnitColorMaterial(playerColors[1]);
        unit8.SetUnitColorMaterial(playerColors[1]);
        unit9.SetUnitColorMaterial(playerColors[0]);
        unit10.SetUnitColorMaterial(playerColors[0]);
        unit11.SetUnitColorMaterial(playerColors[1]);
        unit12.SetUnitColorMaterial(playerColors[1]);

        units.Add(unit1);
        units.Add(unit2);
        units.Add(unit3);
        units.Add(unit4);
        units.Add(unit5);
        units.Add(unit6);
        units.Add(unit7);
        units.Add(unit8);
        units.Add(unit9);
        units.Add(unit10);
        units.Add(unit11);
        units.Add(unit12);
        //////////////////////////////////
        /* 
        Debug.Log("Unsorted: \n");
        foreach (Unit u in units)
        {
            Debug.Log(u);
        }
        */

        units.Sort();
        UnitQueueUIUpdate();

        /* 
        Debug.Log("Sorted: \n");
        foreach (Unit u in units)
        {
            Debug.Log(u);
        }
        */
        turnPointer = units.Count - 1;
        SwitchToUnit();
        SetLayer(currentUnit.gameObject, 15);
    }

    public Unit SpawnTroop(Unit troop, IPlayer controller, ITile tile, HexDirection orientation)
    {
        Unit unit = Instantiate(troop);

        unit.Direction = (int) orientation;
        unit.transform.rotation = Quaternion.Euler(0, orientation.DirectionRotation(), 0);
        Vector3 tilePosition = new Vector3(tile.PosX, tile.PosY, tile.PosZ);
        unit.transform.position = tilePosition + Vector3.up * unit.DisplacementY;

        hexControl.PlaceUnit(unit, tile.X, tile.Z);
        unit.Initialize(controller, hexControl, this);
        return unit;
    }

    public void EndUnitTurn()
    {   
        currentUnit.CurrentActionPoints = 0;
        turnPointer--;
        if (turnPointer < 0)
        {
            units.Sort();   
            turnPointer = units.Count - 1;
            foreach (Unit unit in units)
            {
                unit.CurrentActionPoints = unit.MaxActionPoints;
            }
            UnitQueueUIUpdate();
        }

        if (turnPointer < units.Count)
        {
            SetLayer(currentUnit.gameObject, 8);
            SwitchToUnit();
            UnitQueueUIUpdate();
        }
        else
        {
            Debug.LogError("Something went wrong with the turn order!");
        }
        
        
    }

    private void SwitchToUnit()
    { 
            currentUnit = units[turnPointer];
            UpdateEconomyPanel(players, units, currentUnit);
            currentUnit.CurrentActionPoints = currentUnit.MaxActionPoints;
            unitController.SetSelectedUnit(currentUnit);
            SetLayer(currentUnit.gameObject, 15);
            CameraController cam = Camera.main.GetComponent<CameraController>();
            UnitCameraController unitCam = unitCamera.GetComponent<UnitCameraController>();
            cam.CenterOn(currentUnit.transform.position);
            unitCam.CenterOn(currentUnit.transform.position);
    }

    public void RemoveUnit(Unit unit)
    {
        
        int index = units.IndexOf(unit);
        if (index > -1)
        {
            units.RemoveAt(index);
            unitsSorted.RemoveAt(index);
            UnitOnDeathQueueUIUpdate();
            if (index <= turnPointer)
                turnPointer--;
        }
        if(unit == currentUnit){
            SetLayer(currentUnit.gameObject, 8);
            SwitchToUnit();
        }
    }

    public Unit CurrentUnit
    {
        get
        {
            return currentUnit;
        }
    }

    private void UnitQueueUIUpdate()
    {   
        UnitQueueHide();
        unitsSorted = units.OrderBy(unit => unit.CurrentActionPoints).ThenBy(unit => unit.MaxActionPoints).ToList();
        unitsSorted.Reverse(); 
        for (int i = 0; i < unitsSorted.Count; i++)
        {   
            if(unitsSorted[i] != null && i < unitQueueLimit)
            {
                unitQueuePanels[i].SetActive(true);
                unitPlayerTexts[i].GetComponent<Text>().text = unitsSorted[i].Controller.Team.ToString();
                unitTypeTexts[i].GetComponent<Text>().text = unitsSorted[i].UnitName;
            }
        }
    }
    private void UnitOnDeathQueueUIUpdate()
    {
        UnitQueueHide();
        unitsSorted = units.OrderBy(unit => unit.CurrentActionPoints).ThenBy(unit => unit.MaxActionPoints).ToList();
        unitsSorted.Reverse();
        thisUnit = currentUnit;
        unitsSorted.Remove(currentUnit);
        unitsSorted.Insert(0,thisUnit);
        for (int i = 0; i < unitsSorted.Count; i++)
        {   
            if(unitsSorted[i] != null && i < unitQueueLimit)
            {
                unitQueuePanels[i].SetActive(true);
                unitPlayerTexts[i].GetComponent<Text>().text = unitsSorted[i].Controller.Team.ToString();
                unitTypeTexts[i].GetComponent<Text>().text = unitsSorted[i].UnitName;
            }
        }
    }
    private void UnitQueueHide(){
        foreach (GameObject unitQueuePanel in unitQueuePanels)
        {
            unitQueuePanel.SetActive(false);
        }
    }

    public void HideQueuePanel(){
        if(queuePanel.activeSelf == true){
            queuePanel.SetActive(false);
        }
        else{
            queuePanel.SetActive(true);
        }
    }
    public void HideUnitPanels(){
        foreach (GameObject unitPanel in unitPanels)
        {
            if(unitPanel.activeSelf == true){
            unitPanel.SetActive(false);
        }
        else{
            unitPanel.SetActive(true);
        }
        }
       
    }
    public void HideMiniMapPanel(){
        
        if(miniMapPanel.activeSelf == true){
            miniMapPanel.SetActive(false);
            uiControlPanel.localPosition = miniMapPanel.GetComponent<RectTransform>().localPosition;
        }
        else{

            miniMapPanel.SetActive(true);
            uiControlPanel.localPosition = oldControlPanelPosition;
        }
    }
    public void HideCommandPanel(){
        if(commandPanel.activeSelf == true){
            commandPanel.SetActive(false);
        }
        else{
            commandPanel.SetActive(true);
        }
    }

    /*private List<Unit> Shift(List<Unit> units)
    {
        Unit[] unitsArray = units.ToArray();
        Unit[] tArray = new Unit[unitsArray.Length];
        Array.Copy(unitsArray, 1, tArray, 0, unitsArray.Length - 1);
        tArray[tArray.Length - 1] = currentUnit;
        List<Unit> tList = tArray.ToList();
        return tList;
    }*/

    private void SetLayer(GameObject unit, int layerNumber)
    {
        foreach (Transform trans in unit.GetComponentsInChildren<Transform>(true))
        {
            if(trans.gameObject.layer != 13)
            trans.gameObject.layer = layerNumber;
        }
    }

    private void UpdateEconomyPanel(List<IPlayer> players, List<Unit> units, Unit selectedUnit){
        foreach (Player player in players)
        {
            if(selectedUnit.Controller.Team == player.Team){
                economyPanelTexts[0].GetComponent<Text>().text = "Money:    " + player.GetResource(ResourceType.Money).ToString();
                economyPanelTexts[1].GetComponent<Text>().text = "Oil:    " + player.GetResource(ResourceType.Oil).ToString();
                economyPanelTexts[2].GetComponent<Text>().text = "Steel:    " + player.GetResource(ResourceType.Steel).ToString();

                int playerUnitCount = 0;
                foreach (Unit unit in units)
                {
                    if(player.Team == unit.Controller.Team){
                        playerUnitCount++;
                    }
                }
                economyPanelTexts[3].GetComponent<Text>().text = "Units:    " + playerUnitCount;
                economyPanelTexts[4].GetComponent<Text>().text = "Player:    " + player.Team;
            }
        }
    }
}
