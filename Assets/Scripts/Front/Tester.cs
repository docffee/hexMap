using Assets.Scripts.HexImpl;
using GraphAlgorithms;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Tester : MonoBehaviour
{
    private void Start()
    {
        HexGrid grid = FindObjectOfType<HexGrid>();
        HexEngine engine = new HexEngine(grid.SizeX, grid.SizeZ, grid);

        GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");

        UnityUnit unit1 = units[0].GetComponent<UnityUnit>();
        UnityUnit unit2 = units[1].GetComponent<UnityUnit>();

        ITile<HexNode> tile = engine.GetTile(19, 12);
        ITile<HexNode> tile2 = engine.GetTile(21, 12);
        Vector3 tilePosition = new Vector3(tile.WorldPosX, tile.WorldPosY, tile.WorldPosZ);
        Vector3 tilePosition2 = new Vector3(tile2.WorldPosX, tile2.WorldPosY, tile2.WorldPosZ);
        unit1.transform.position = tilePosition + Vector3.up * unit1.DisplacementY;
        unit2.transform.position = tilePosition2 + Vector3.up * unit2.DisplacementY;

        engine.PlaceUnit(unit1, tile.X, tile.Z);
        engine.PlaceUnit(unit2, tile2.X, tile2.Z);
        Camera.main.transform.position = new Vector3(tilePosition.x, Camera.main.transform.position.y, tilePosition.z - 35);

        //unit.Move(engine.MoveUnit(unit, tile, engine.GetTile(1, 0)));

        //int index = 23;
        //ITile<HexNode>[] cells = grid.GenerateTiles(sizeX, sizeZ);
        //HexNode[] nodes = {
        //    new HexNode(HexDirection.NE, cells[index]),
        //    new HexNode(HexDirection.E, cells[index]),
        //    new HexNode(HexDirection.SE, cells[index]),
        //    new HexNode(HexDirection.SW, cells[index]),
        //    new HexNode(HexDirection.W, cells[index]),
        //    new HexNode(HexDirection.NW, cells[index])
        //};

        //foreach (HexNode node in nodes)
        //{
        //    Debug.Log("From: " + node + " || to Front: " + engine.GetNodeInFront(node));
        //    Debug.Log("From: " + node + " || to Back: " + engine.GetNodeBehind(node));
        //}

        //Debug.Log("NE: " + HexDirection.NE.DirectionRotation());
        //Debug.Log("E: " + HexDirection.E.DirectionRotation());

        /*
        ITile<HexNode>[] cells = grid.GenerateTiles(sizeX, sizeZ);
        IEnumerable<IPathNode<HexNode>> path = engine.MoveUnit(new DummyUnit(), cells[0], cells[99]);

        foreach (IPathNode<HexNode> node in path)
        {
            Debug.Log(node.GetNode().ToString());
        }
        */
    }
}
