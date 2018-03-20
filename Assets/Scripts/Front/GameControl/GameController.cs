using Assets.Scripts.HexImpl;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private Player playerPrefab;
    [SerializeField] private int numberOfPlayers;
    [SerializeField] private int startMoney;

    [Header ("This is only temporary")]
    [SerializeField] private UnityUnit[] allUnits;

    HexControl hexControl;
    private List<IPlayer> players;

    private void Start()
    {
        HexGrid grid = FindObjectOfType<HexGrid>();
        hexControl = new HexControl(grid.SizeX, grid.SizeZ, grid);
        players = new List<IPlayer>();

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

        SpawnTroop(allUnits[0], players[0], tile, HexDirection.E);
        SpawnTroop(allUnits[2], players[0], tile2, HexDirection.E);
        SpawnTroop(allUnits[0], players[1], tile3, HexDirection.W);
        SpawnTroop(allUnits[2], players[1], tile4, HexDirection.W);

        Vector3 tilePosition = new Vector3(tile.WorldPosX, tile.WorldPosY, tile.WorldPosZ);
        Camera.main.transform.position = new Vector3(tilePosition.x, Camera.main.transform.position.y, tilePosition.z - 35);
        //////////////////////////////////
    }

    public void SpawnTroop(UnityUnit troop, IPlayer controller, ITile tile, HexDirection orientation)
    {
        UnityUnit unit = Instantiate(troop);

        unit.Direction = (int) orientation;

        Debug.Log(orientation.DirectionRotation());

        unit.transform.rotation = Quaternion.Euler(0, orientation.DirectionRotation(), 0);
        Vector3 tilePosition = new Vector3(tile.WorldPosX, tile.WorldPosY, tile.WorldPosZ);
        unit.transform.position = tilePosition + Vector3.up * unit.DisplacementY;

        hexControl.PlaceUnit(unit, tile.X, tile.Z);
        unit.Initialize(controller);
    }
}
