using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// script to attach to the camera object to have it follow the character on the XY plane
public class CameraFollow2D : MonoBehaviour {

	// the item to follow on the XY plane
	public Transform target;
	float YOffset;

	private void Start() {
		// store the difference between the camera Y position and the target Y position
		YOffset = transform.position.y - target.position.y;
	}

	void Update() {
		// update the camera position following the target position
		transform.position = new Vector3(target.position.x, target.position.y + YOffset, transform.position.z);
    }
}
