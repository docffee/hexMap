using Assets.Scripts.HexImpl;
using Assets.Scripts.Interfaces;
using GraphAlgorithms;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBlimp : UnityUnit
{
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

    public override void Move(IEnumerable<IPathNode<HexNode>> path, IReady controller)
    {
        StartCoroutine(MoveWaiter(path, controller));
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

    public override bool Flying
    {
        get
        {
            return flying;
        }
    }
}
