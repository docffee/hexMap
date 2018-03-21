using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCameraController : MonoBehaviour {
	private Vector3 desiredPosition;
	
	void Start () {
		desiredPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void CenterOn(Vector3 position)
    {
        Vector3 next = new Vector3(position.x, transform.position.y, position.z);
        desiredPosition = next;
        transform.position = next;
    }
}
