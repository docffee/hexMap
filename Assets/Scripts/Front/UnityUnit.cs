using Assets.Scripts.HexImpl;
using Assets.Scripts.Interfaces;
using GraphAlgorithms;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnityUnit : MonoBehaviour, IUnit<HexNode>
{
    [SerializeField] protected float moveTime = 1.2f;
    [SerializeField] protected float rotateTime = 0.2f;
    [SerializeField] protected float displacementY = 1;
    [SerializeField] protected bool flying;
    [SerializeField] protected bool canWalkBackwards;
    [SerializeField] protected float maxMovePoints = 8;
    [SerializeField] protected float currentMovePoints = 8;
    [SerializeField] protected float damage = 1;
    [SerializeField] protected float health = 5;

    [SerializeField] protected Sprite icon;

    protected ITile<HexNode> tile;
    protected HexDirection orientation;
    protected bool performingAction = false;

    //Remove abstract at some point
    public Sprite Icon
    {
        get { return icon; }
    }
    public abstract int MaxActionPoints { get; }
    public abstract int CurrentActionPoints { get; }
    public abstract float MaxMovePoints { get; set; }
    public abstract float CurrentMovePoints { get; set; }
    public abstract int Direction { get; }
    public abstract float RotateCost { get; }
    public abstract ITile<HexNode> Tile { get; set; }

    public abstract IWalkable GetTerrainWalkability(ITerrain terrain);

    public abstract void Move(IEnumerable<IPathNode<HexNode>> path, IReady controller);
    public abstract bool PerformingAction();

    public abstract float DisplacementY { get; }
    public abstract bool Flying { get; }

    public bool CanWalkBackwards
    {
        get
        {
            return canWalkBackwards;
        }
    }

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

            if (cost <= currentMovePoints)
                yield return Step(node);
            else
                break;

            lastNode = enumerator.Current;
        }

        currentMovePoints -= lastNode.GetCost();

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
}
