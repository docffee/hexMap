using Assets.Scripts.HexImpl;
using GraphAlgorithms;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestUnit : MonoBehaviour, IUnit<HexNode>
{
    [SerializeField] private float moveTime = 1.2f;
    [SerializeField] private float rotateTime = 0.2f;
    [SerializeField] private float displacementY = 1;

    private ITile<HexNode> tile;
    private HexDirection orientation;
    private bool performingAction = false;

    float maxMovePoints = 8;
    [SerializeField] float currentMovePoints = 8;

    public void Move(IEnumerable<IPathNode<HexNode>> path)
    {
        StartCoroutine(MoveWaiter(path));
    }

    private IEnumerator MoveWaiter(IEnumerable<IPathNode<HexNode>> path)
    {
        if (path == null)
        {
            Debug.Log("Can't find path!!");
            yield break;
        }
        performingAction = true;

        IEnumerator<IPathNode<HexNode>> enumerator = path.GetEnumerator();
        enumerator.MoveNext(); // Skips first;
        while(enumerator.MoveNext())
        {
            IPathNode<HexNode> node = enumerator.Current;
            float cost = node.GetCost();

            if (cost <= currentMovePoints)
                yield return Step(node);
            else
                break;
        }

        performingAction = false;
    }

    private IEnumerator Step(IPathNode<HexNode> pathNode)
    {
        HexNode node = pathNode.GetNode();
        currentMovePoints -= pathNode.GetCost();

        //if (tile.X == node.X && tile.Z == node.Z && orientation == node.Direction)
        //    yield break;

        if (node.Direction != orientation)
            yield return Rotate(node);
        else
            yield return Walk(node);
    }

    private IEnumerator Walk(HexNode node)
    {
        ITile<HexNode> tile = node.Tile;

        Vector3 startPoint = transform.position;
        Vector3 nodePoint = new Vector3(tile.WorldPosX, tile.WorldPosY, tile.WorldPosZ) + Vector3.up * displacementY;

        float elapsedTime = 0;
        while (elapsedTime < moveTime)
        {
            Vector3 between = Vector3.Lerp(startPoint, nodePoint, elapsedTime / moveTime);
            transform.position = between;
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        transform.position = nodePoint;
        HexEngine.Singleton.MoveUnit(this, tile.X, tile.Z, node.X, node.Z);
    }

    private IEnumerator Rotate(HexNode node)
    {
        float elapsedTime = 0;
        Quaternion start = transform.rotation;
        Vector3 startEuler = start.eulerAngles;

        float yIncrement = HexUtil.StepRotation(orientation, node.Direction);
        Quaternion end = Quaternion.Euler(startEuler.x, startEuler.y + yIncrement, startEuler.z);

        while (elapsedTime < rotateTime)
        {
            Quaternion between = Quaternion.Lerp(start, end, elapsedTime / rotateTime);
            transform.rotation = between;
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        orientation = node.Direction;
        transform.rotation = end;
    }

    public bool PerformingAction()
    {
        return performingAction;
    }

    public float GetTerrainModifier(ITerrain terrain)
    {
        switch (terrain.Name)
        {
            case "Grass":
                return 1;
            case "Forest":
                return 2;
            case "Mountain":
                return float.MaxValue;
            case "Water":
                return float.MaxValue;
            case "Sand":
                return 3;
            default:
                return float.MaxValue;
        }
    }

    public float RotateCost
    {
        get
        {
            return 0.2f;
        }
    }

    public int MaxActionPoints
    {
        get
        {
            return 2;
        }
    }

    public int CurrentActionPoints
    {
        get
        {
            return 2;
        }
    }

    public int Direction
    {
        get
        {
            return (int)orientation;
        }
    }

    public float DisplacementY
    {
        get
        {
            return displacementY;
        }
    }

    public ITile<HexNode> Tile
    {
        get
        {
            return tile;
        }

        set
        {
            tile = value;
        }
    }

    public float MaxMovePoints
    {
        get { return maxMovePoints; }

        set { maxMovePoints = value; }
    }

    public float CurrentMovePoints
    {
        get { return currentMovePoints; }

        set { currentMovePoints = value; }
    }
}
