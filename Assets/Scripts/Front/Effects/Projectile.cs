﻿using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 1;

    public static float elapsedTime;

    public void Initialize(Vector3 targetPos)
    {
        float dist = Vector3.Distance(transform.position, targetPos);
        elapsedTime = dist/speed;
        StartCoroutine(MoveTowards(targetPos));
    }

    private IEnumerator MoveTowards(Vector3 targetPos)
    {
        Vector3 dir = VectorDirection(transform.position, targetPos);
        
        while (!ArrivedAtTarget(targetPos, dir))
        {
            transform.position += dir * speed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        elapsedTime = 0;
        yield return new WaitForEndOfFrame();
        Destroy(gameObject);
    }

    private bool ArrivedAtTarget(Vector3 targetPos, Vector3 dir)
    {
        return FurtherThanOrEqualTo(transform.position.x, targetPos.x, dir.x)
            && FurtherThanOrEqualTo(transform.position.y, targetPos.y, dir.y)
            && FurtherThanOrEqualTo(transform.position.z, targetPos.z, dir.z);
    }

    private Vector3 VectorDirection(Vector3 start, Vector3 end)
    {
        Vector3 dif = end - start;
        return Vector3.Normalize(dif);
    }

    private bool FurtherThanOrEqualTo(float point, float target, float dir)
    {
        if (dir < 0)
            return point <= target;

        return point >= target;
    }
}
