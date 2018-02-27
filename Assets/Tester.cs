using Assets.Scripts.HexImpl;
using GraphAlgorithms;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Tester : MonoBehaviour
{
    private void Start()
    {
        int sizeX = 10;
        int sizeZ = 10;

        HexGrid grid = FindObjectOfType<HexGrid>();
        HexEngine engine = new HexEngine(sizeX, sizeZ, grid);

        //Debug.Log(engine.GetTile(5, 5));

        ITile<HexNode>[] cells = grid.GenerateTiles(sizeX, sizeZ);
        IEnumerable<IPathNode<HexNode>> path = engine.MoveUnit(new DummyUnit(), cells[0], cells[99]);

        foreach (IPathNode<HexNode> node in path)
        {
            Debug.Log(node.GetNode().ToString());
        }
    }
}
