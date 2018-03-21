using System.Collections;
using UnityEngine;

public class Projectile2 : MonoBehaviour
{
    [SerializeField] private float speed = 1;

    public void Initialize(Vector3 targetPos)
    {
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
