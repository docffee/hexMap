using Assets.Scripts.HexImpl;
using Assets.Scripts.Interfaces;
using GraphAlgorithms;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Assets.Scripts.Front.Actions;

public abstract class Unit : MonoBehaviour, IUnit, ICombatable, IEquatable<Unit>, IComparable<Unit>
{
    [Header ("---Visuals---")]
    [SerializeField] private string unitName = "New Unit";
    [SerializeField] private Sprite icon = null;
    [SerializeField] private float moveTime = 1.2f;
    [SerializeField] private float rotateTime = 0.2f;
    [SerializeField] private float displacementY = 1;
    [SerializeField] private GameObject explosion;
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private MeshRenderer[] mainBodyRenderers;

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
    protected IPlayer controller;
    protected ITileControl<HexNode> hexControl;
    protected GameController gameController;
    protected IAction[] actions;

    public abstract IWalkable GetTerrainWalkability(ITerrain terrain);
    public abstract void Move(IEnumerable<IPathNode<HexNode>> path, IReady controller);
    public abstract bool CanRetaliate();

    public void Initialize(IPlayer controller, ITileControl<HexNode> hexControl, GameController gameController)
    {
        this.controller = controller;
        this.hexControl = hexControl;
        this.gameController = gameController;
        currentActionPoints = maxActionPoints;

        actions = new IAction[] { new Attack(this) };
    }

    public virtual void OnAttack(ICombatable target)
    {
        if (projectilePrefab != null)
        {
            Projectile instance = Instantiate(projectilePrefab, transform.position, transform.rotation);
            Vector3 targetPos = new Vector3(target.PosX, target.PosY, target.PosZ);
            instance.Initialize(targetPos);
        }
    }

    public virtual void OnDeath()
    {
        if (flying)
            tile.AirUnitOnTile = null;
        else
            tile.UnitOnTile = null;

        gameController.RemoveUnit(this);
        Invoke("InstantiateExplosion", Projectile.elapsedTime);
        Destroy(gameObject, Projectile.elapsedTime);
    }
    public void InstantiateExplosion(){
        Instantiate(explosion, transform.position, transform.rotation);
    }

    public void SetUnitColorMaterial(Material color)
    {
        foreach (MeshRenderer rend in mainBodyRenderers)
        {
            rend.material = color;
        }
    }

    protected IEnumerator MoveWaiter(IEnumerable<IPathNode<HexNode>> path, IReady controller)
    {
        if (path == null)
        {
            Debug.Log("Can't find path!!");
            yield break;
        }

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
        Vector3 nodePoint = new Vector3(tile.PosX, tile.PosY, tile.PosZ) + Vector3.up * displacementY;

        float elapsedTime = 0;
        while (elapsedTime < moveTime)
        {
            Vector3 between = Vector3.Lerp(startPoint, nodePoint, elapsedTime / moveTime);
            transform.position = between;
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        transform.position = nodePoint;
        hexControl.MoveUnit(this, tile.X, tile.Z, node.X, node.Z);
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

    public bool IsTilePassable(ITile tile)
    {
        if (flying)
        {
            if (tile.AirUnitOnTile != null)
                return Equals(tile.AirUnitOnTile);

            return true; 
        }

        if (tile.UnitOnTile != null)
            return Equals(tile.UnitOnTile);  

        return tile.UnitOnTile == null;
    }

    public int CompareTo(Unit other)
    {
        return (int)maxActionPoints - (int)other.maxActionPoints;
    }

    public bool Equals(Unit other)
    {
        if (other == null)
            return false;

        return GetInstanceID() == other.GetInstanceID();
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

    public float PosX
    {
        get
        {
            return transform.position.x;
        }
    }

    public float PosY
    {
        get
        {
            return transform.position.y;
        }
    }

    public float PosZ
    {
        get
        {
            return transform.position.z; ;
        }
    }

    public IAction[] Actions
    {
        get
        {
            return actions;
        }
    }
}
