using Assets.Scripts.HexImpl;
using GraphAlgorithms;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestUnit2 : UnityUnit
{
    [SerializeField] private float moveTime = 1.2f;
    [SerializeField] private float rotateTime = 0.2f;
    [SerializeField] private float displacementY = 1;

    private ITile<HexNode> tile;
    private HexDirection orientation;
    private bool performingAction = false;

    float maxMovePoints = 8;
    [SerializeField] float currentMovePoints = 8;

    private void Start()
    {
        StartCoroutine(FloatingRoutine());
    }

    private IEnumerator FloatingRoutine()
    {
        float offset = 1.8f;
        float time = 1.2f;
        int mod = 1;
        while (true)
        {
            float elapsedTime = 0;
            float startY = transform.position.y;
            float endY = startY + (offset * mod);
            while (elapsedTime < time)
            {
                Vector3 posBefore = new Vector3(transform.position.x, startY, transform.position.z);
                Vector3 posAfter = new Vector3(transform.position.x, endY, transform.position.z);
                Vector3 next = Vector3.Lerp(posBefore, posAfter, elapsedTime / time);
                transform.position = next;
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            transform.position = new Vector3(transform.position.x, endY, transform.position.z);
            mod = mod * -1;
            yield return new WaitForEndOfFrame();
        }
    }

    public override void Move(IEnumerable<IPathNode<HexNode>> path)
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
        IPathNode<HexNode> lastNode = enumerator.Current;
        while (enumerator.MoveNext())
        {
            IPathNode<HexNode> node = enumerator.Current;
            lastNode = enumerator.Current;
            float cost = node.GetCost();

            if (cost <= currentMovePoints)
                yield return Step(node);
            else
                break;
        }

        currentMovePoints -= lastNode.GetCost();
        performingAction = false;
    }

    private IEnumerator Step(IPathNode<HexNode> pathNode)
    {
        HexNode node = pathNode.GetNode();

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

    public override bool PerformingAction()
    {
        return performingAction;
    }

    public override IWalkable GetTerrainWalkability(ITerrain terrain)
    {
        switch (terrain.Name)
        {
            case "Grass":
                return new Walkable(2, true);
            case "Forest":
                return new Walkable(2, true);
            case "Mountain":
                return new Walkable(2, true);
            case "Water":
                return new Walkable(2, true);
            case "Sand":
                return new Walkable(2, true);
            default:
                return new Walkable(0, false);
        }
    }

    public override float RotateCost
    {
        get
        {
            return 0.2f;
        }
    }

    public override int MaxActionPoints
    {
        get
        {
            return 2;
        }
    }

    public override int CurrentActionPoints
    {
        get
        {
            return 2;
        }
    }

    public override int Direction
    {
        get
        {
            return (int)orientation;
        }
    }

    public override float DisplacementY
    {
        get
        {
            return displacementY;
        }
    }

    public override ITile<HexNode> Tile
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

    public override float MaxMovePoints
    {
        get { return maxMovePoints; }

        set { maxMovePoints = value; }
    }

    public override float CurrentMovePoints
    {
        get { return currentMovePoints; }

        set { currentMovePoints = value; }
    }
}
