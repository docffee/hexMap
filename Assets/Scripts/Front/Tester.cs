﻿using Assets.Scripts.HexImpl;
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

        TestUnit unit = GameObject.FindGameObjectWithTag("Unit").GetComponent<TestUnit>();
        ITile<HexNode> tile = engine.GetTile(0, 0);
        Vector3 tilePosition = new Vector3(tile.WorldPosX, tile.WorldPosY, tile.WorldPosZ);
        unit.transform.position = tilePosition + Vector3.up * unit.DisplacementY;

        engine.PlaceUnit(unit, tile.X, tile.Z);

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