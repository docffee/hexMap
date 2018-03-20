using Assets.Scripts.HexImpl;
using UnityEngine;

class Loader : MonoBehaviour
{
    private void Start()
    {
        HexGrid grid = FindObjectOfType<HexGrid>();
        HexEngine engine = new HexEngine(grid.SizeX, grid.SizeZ, grid);

        UnityUnit[] units = FindObjectsOfType<UnityUnit>();

        UnityUnit unit1 = units[0];
        UnityUnit unit2 = units[1];
        UnityUnit unit3 = units[2];
        UnityUnit unit4 = units[3];

        foreach (UnityUnit unit in units)
        {
            unit.CurrentActionPoints = unit.MaxActionPoints;
            unit.CurrentHealth = unit.MaxHealth;
        }

        ITile<HexNode> tile = engine.GetTile(19, 12);
        ITile<HexNode> tile2 = engine.GetTile(21, 12);
        ITile<HexNode> tile3 = engine.GetTile(23, 12);
        ITile<HexNode> tile4 = engine.GetTile(17, 12);
        Vector3 tilePosition = new Vector3(tile.WorldPosX, tile.WorldPosY, tile.WorldPosZ);
        Vector3 tilePosition2 = new Vector3(tile2.WorldPosX, tile2.WorldPosY, tile2.WorldPosZ);
        Vector3 tilePosition3 = new Vector3(tile3.WorldPosX, tile3.WorldPosY, tile3.WorldPosZ);
        Vector3 tilePosition4 = new Vector3(tile4.WorldPosX, tile4.WorldPosY, tile4.WorldPosZ);
        unit1.transform.position = tilePosition + Vector3.up * unit1.DisplacementY;
        unit2.transform.position = tilePosition2 + Vector3.up * unit2.DisplacementY;
        unit3.transform.position = tilePosition3 + (Vector3.up * unit3.DisplacementY);
        unit4.transform.position = tilePosition4 + (Vector3.up * unit4.DisplacementY);

        engine.PlaceUnit(unit1, tile.X, tile.Z);
        engine.PlaceUnit(unit2, tile2.X, tile2.Z);
        engine.PlaceUnit(unit3, tile3.X, tile3.Z);
        engine.PlaceUnit(unit4, tile4.X, tile4.Z);
        Camera.main.transform.position = new Vector3(tilePosition.x, Camera.main.transform.position.y, tilePosition.z - 35);
    }
}
