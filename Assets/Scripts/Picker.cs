using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Picker : MonoBehaviour {

	// KeyCode variables store references to keyboard keys and joystick buttons
	// we use a public KeyCode variable to be able to set which key we want to trigger the "pick up"
	public KeyCode pickupKey;

	// to allow the player to carry something, we'll need it to have a child transform to use as a referenfe to where to keep the carried object
	public Transform pickupTarget;

	// the Update funtion runs every frame
	void Update () {

		// check if our pick up key was pressed in this frame
		// Input.GetKeyDown(KeyCode key) returns true only during the FIRST frame a key has been pressed
		// in order to be triggered again, such key needs to be released first
		if (Input.GetKeyDown(pickupKey)) {

			// create RaycastHit local variable to store the outcome of the upcoming SphereCast function
			RaycastHit hit;

			// bool Physics.SphereCast(Vector3 origin, float radius, Vectpor3 direction, out RaycastHit hitInfo, float maxDistance)
			// casts a ray of spheres from a given point to a given direction, storing the result (the "out" parameter) into a RaycastHit variable
			// as it's a bool function it also returns whether true or false depending if the ray hit something or not
			// if it hit something
			if (Physics.SphereCast(transform.position, 1f, transform.forward, out hit, 1f)) {
				
				// check if that collider has a pickable component
				// to do so just use the GetComponent function on the hit.transform
				// if the GetComponent function doesnt find the required component, it will just return null
				Pickable p = hit.transform.GetComponent<Pickable>();

				// if it does (so the p variable is not null) execute the pickup code
				if (p != null) {

					// get the reference to the pickable Rigidbody and deactivate it
					Rigidbody cubeRigidbody = hit.transform.GetComponent<Rigidbody>();
					// to "deactivate" a Rigidbody just set it to "kinematic" and disable its usage of gravity
					cubeRigidbody.isKinematic = true;
					cubeRigidbody.useGravity = false;

					// to have the pickable move with the player make cube child of pickable target transform 
					hit.transform.parent = pickupTarget;
					// and move it to LOCAL 0,0,0
					hit.transform.localPosition = Vector3.zero;
				}

			}

		}

		// draws a ray in the scene view (to see if we're raycasting in the right direction and from the right place)
		Debug.DrawRay(transform.position, transform.forward, Color.blue);

	}

}