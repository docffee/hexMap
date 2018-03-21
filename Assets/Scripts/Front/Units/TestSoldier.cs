using Assets.Scripts.HexImpl;
using GraphAlgorithms;
using System.Collections.Generic;
using Assets.Scripts.Interfaces;
using UnityEngine;
using System.Collections;

public class TestSoldier : Unit
{
    private bool canRetaliate = true;

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
                return new Walkable(1, true);
            case "Forest":
                return new Walkable(1, true);
            case "Mountain":
                return new Walkable(0, false);
            case "Water":
                return new Walkable(0, false);
            case "Sand":
                return new Walkable(1.5f, true);
            default:
                return new Walkable(0, false);
        }
    }

    public override bool CanRetaliate()
    {
        return canRetaliate;
    }

    public override void OnAttack(ICombatable target)
    {
        Vector3 targetPosition = (target as Unit).transform.position;
        StartCoroutine(AttackAnimation(targetPosition));
    }

    private IEnumerator AttackAnimation(Vector3 targetPosition)
    {
        yield break;
    }
}