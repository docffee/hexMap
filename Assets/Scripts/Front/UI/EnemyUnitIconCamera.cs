﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnitIconCamera : MonoBehaviour {
	
	public void CenterOn(Vector3 position)
    {
        Vector3 next = new Vector3(position.x, transform.position.y, position.z);
        Vector3 desiredPosition = next;
        transform.position = next;
    }
}
