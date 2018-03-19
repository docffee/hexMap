using Assets.Scripts.HexImpl;
using Assets.Scripts.Interfaces;
using GraphAlgorithms;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class UnityUnit : MonoBehaviour, IUnit<HexNode>
{
    [SerializeField] protected float moveTime = 1.2f;
    [SerializeField] protected float rotateTime = 0.2f;
    [SerializeField] protected float displacementY = 1;
    [SerializeField] protected bool flying;
    [SerializeField] protected bool canWalkBackwards;
    [SerializeField] protected float rotateCost;
    [SerializeField] protected float maxActionPoints = 1;
    [SerializeField] protected float currentActionPoints = 1;
    [SerializeField] protected float damage = 1;
    [SerializeField] protected float health = 1;
    public string unitName;

    [SerializeField] protected Sprite icon;

    protected ITile<HexNode> tile;
    protected HexDirection orientation;
    protected bool performingAction = false;

    public abstract IWalkable GetTerrainWalkability(ITerrain terrain);

    public abstract ITile<HexNode> Tile { get; set; }
    public abstract void Move(IEnumerable<IPathNode<HexNode>> path, IReady controller);
    public abstract bool PerformingAction();

    protected IEnumerator MoveWaiter(IEnumerable<IPathNode<HexNode>> path, IReady controller)
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
            float cost = node.GetCost();

            if (cost <= currentActionPoints)
                yield return Step(node);
            else
                break;

            lastNode = enumerator.Current;
        }

        currentActionPoints -= lastNode.GetCost();

        controller.Ready();
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

    //Remove abstract at some point
    public Sprite Icon
    {
        get { return icon; }
    }

    public float MaxActionPoints
    {
        get { return maxActionPoints; }
        set { maxActionPoints = value; }
    }

    public float CurrentActionPoints
    {
        get { return currentActionPoints; }
        set { currentActionPoints = value; }
    }

    public int Direction
    {
        get { return (int) orientation; }
        set { orientation = (HexDirection) value; }
    }

    public float RotateCost
    {
        get { return rotateCost; }
    }

    public float DisplacementY
    {
        get { return displacementY; }
    }

    public bool Flying
    {
        get { return flying; }
    }

    public bool CanWalkBackwards
    {
        get
        {
            return canWalkBackwards;
        }
    }
}
