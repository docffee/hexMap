using Assets.Scripts.HexImpl;
using GraphAlgorithms;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestUnit : MonoBehaviour, IUnit<HexNode>
{
    [SerializeField] private float moveSpeed = 1.2f;
    [SerializeField] private float rotateSpeed = 0.2f;
    [SerializeField] private float displacementY = 1.2f;

    private ITile<HexNode> tile;
    private HexDirection orientation;
    private bool performingAction = false;

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
        foreach (IPathNode<HexNode> node in path)
        {
            Debug.Log(node.GetNode());
            yield return Step(node);
        }
        performingAction = false;
    }

    private IEnumerator Step(IPathNode<HexNode> pathNode)
    {
        HexNode node = pathNode.GetNode();

        if (node.Direction != orientation)
            yield return Rotate(node);
        else
            yield return Walk(node.Tile);
    }

    private IEnumerator Walk(ITile<HexNode> tile)
    {
        Vector3 startPoint = transform.position;
        Vector3 nodePoint = new Vector3(tile.WorldPosX, tile.WorldPosY, tile.WorldPosZ) + Vector3.up * displacementY;

        float elapsedTime = 0;
        while (elapsedTime < moveSpeed)
        {
            Vector3 between = Vector3.Lerp(startPoint, nodePoint, elapsedTime / moveSpeed);
            transform.position = between;
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        transform.position = nodePoint;
    }

    private IEnumerator Rotate(HexNode node)
    {
        float elapsedTime = 0;
        Quaternion start = transform.rotation;
        Vector3 startEuler = start.eulerAngles;

        float yIncrement = HexUtil.StepRotation(orientation, node.Direction);
        Quaternion end = Quaternion.Euler(startEuler.x, startEuler.y + yIncrement, startEuler.z);

        while (elapsedTime < rotateSpeed)
        {
            Quaternion between = Quaternion.Lerp(start, end, elapsedTime / rotateSpeed);
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
}
