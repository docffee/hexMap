using Assets.Scripts.HexImpl;
using Assets.Scripts.Interfaces;
using GraphAlgorithms;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBlimp : Unit
{
    private bool canRetaliate = true;

    private void Start()
    {
        StartCoroutine(FloatingRoutine());
    }

    private IEnumerator FloatingRoutine()
    {
        float offset = 1.8f;
        float time = 1.4f;
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

    public override IWalkable GetTerrainWalkability(ITerrain terrain)
    {
        switch (terrain.Name)
        {
            case "Grass":
                return new Walkable(1, true);
            case "Forest":
                return new Walkable(1, true);
            case "Mountain":
                return new Walkable(1, true);
            case "Water":
                return new Walkable(1, true);
            case "Sand":
                return new Walkable(1, true);
            default:
                return new Walkable(0, false);
        }
    }

    public override bool CanRetaliate()
    {
        return canRetaliate;
    }

    public override bool HideInBuilding() {
        return false;
    }
}