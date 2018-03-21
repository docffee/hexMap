using Assets.Scripts.HexImpl;
using Assets.Scripts.Interfaces;
using GraphAlgorithms;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnityUnit : MonoBehaviour, IUnit, ICombatUnit
{
    [Header ("---Visuals---")]
    [SerializeField] private string unitName = "New Unit";
    [SerializeField] protected Sprite icon = null;
    [SerializeField] protected GameObject explosion;
    [SerializeField] protected float moveTime = 1.2f;
    [SerializeField] protected float rotateTime = 0.2f;
    [SerializeField] protected float displacementY = 1;

    [Header ("---Pathfinding and Actions---")]
    [SerializeField] protected bool flying = false;
    [SerializeField] protected bool canWalkBackwards = false;
    [SerializeField] protected float rotateCost = 0.1f;
    [SerializeField] protected float maxActionPoints = 1;
    protected float currentActionPoints = 1;

    [Header ("---Combat---")]
    [SerializeField] private int currentHealth = 1;
    [SerializeField] private int maxHealth = 1;
    [SerializeField] private int damage = 1;
    [SerializeField] private int range = 1;
    [SerializeField] private float attackActionPointCost;

    protected ITile tile;
    protected HexDirection orientation;
    protected bool performingAction = false;
    protected IPlayer controller;

    public abstract IWalkable GetTerrainWalkability(ITerrain terrain);
    public abstract void Move(IEnumerable<IPathNode<HexNode>> path, IReady controller);
    public abstract bool PerformingAction();
    public abstract bool CanRetaliate();

    public void Initialize(IPlayer controller)
    {
        this.controller = controller;
        currentActionPoints = maxActionPoints;
    }

    public virtual void OnAttack(ICombatUnit target)
    {
        // Do nothing
    }

    public virtual void OnDeath()
    {
        if (flying)
            tile.AirUnitOnTile = null;
        else
            tile.UnitOnTile = null;

        Instantiate(explosion, transform.position, transform.rotation);
        Destroy(gameObject);
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
        ITile tile = node.Tile;

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
        HexControl.Singleton.MoveUnit(this, tile.X, tile.Z, node.X, node.Z);
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

    //#######################################//
    //##### Properties below this point #####//
    //#######################################//

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

    public string UnitName
    {
        get
        {
            return unitName;
        }
    }

    public int CurrentHealth
    {
        get
        {
            return currentHealth;
        }

        set
        {
            currentHealth = value;
        }
    }

    public int MaxHealth
    {
        get
        {
            return maxHealth;
        }
    }

    public int Damage
    {
        get
        {
            return damage;
        }
    }

    public int Range
    {
        get
        {
            return range;
        }
    }

    public float AttackActionPointCost
    {
        get
        {
            return attackActionPointCost;
        }
    }

    public ITile Tile
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

    public IPlayer Controller
    {
        get
        {
            return controller;
        }
    }
}
