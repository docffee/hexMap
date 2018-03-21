using UnityEngine;

public class Projectile : MonoBehaviour {

	[SerializeField] private float projectileSpeed;

	private float step;
	private Vector3 endPoint;
	private Transform targetTransform;

	private void Start() {
		projectileSpeed = Vector3.Distance(transform.position, findTarget(targetTransform).position)/1f;
		step = projectileSpeed * Time.deltaTime;
		endPoint = findTarget(targetTransform).position;
	}

	private void Update() 
	{
		transform.position = Vector3.MoveTowards(transform.position, endPoint, step);
	}

	public Transform findTarget(Transform projectileTarget)
	{
		targetTransform = projectileTarget.transform;
		return targetTransform;
	}

}
