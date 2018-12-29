using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class to allow the player to check for pickable items to pick up, carry and deliver
public class Picker : MonoBehaviour {

	// KeyCode variables store references to keyboard keys and joystick buttons
	// we use a public KeyCode variable to be able to set which key we want to trigger the "pick up"
	public KeyCode pickupKey;

	// to allow the player to carry something, we'll need it to have a child transform to use as a referenfe to where to keep the carried object
	public Transform pickupTarget;

	// reference to the currently held Pickable instance
	// if it's null it means the player is currently NOT holding anything
	// if it's NOT null, it gives us direct access to the currently held item's Pickable component
	Pickable heldObject ;

	// the Update funtion runs every frame
	void Update () {

		// check if our pick up key was pressed in this frame
		// Input.GetKeyDown(KeyCode key) returns true only during the FIRST frame a key has been pressed
		// in order to be triggered again, such key needs to be released first
		if (Input.GetKeyDown(pickupKey)) {
			
			// create RaycastHit local variable to store the outcome of the upcoming SphereCast function
			RaycastHit hit;

			// if the player is not holding anything, check if a pickable or a dispenser are nearby
			if (heldObject == null) {

				// bool Physics.SphereCast(Vector3 origin, float radius, Vectpor3 direction, out RaycastHit hitInfo, float maxDistance)
				// casts a ray of spheres from a given point to a given direction, storing the result (the "out" parameter) into a RaycastHit variable
				// as it's a bool function it also returns whether true or false depending if the ray hit something or not
				// if it hit something
				if (Physics.SphereCast(transform.position, 0.5f, transform.forward, out hit, 2f)) {
					// check if that collider has a pickable component
					// to do so just use the GetComponent function on the hit.transform
					// if the GetComponent function doesnt find the required component, it will just return null
					Pickable p = hit.transform.GetComponent<Pickable>();

					// if it does (so the p variable is not null) execute the pickup code
					if (p != null)
						Pick(p);

					// if the collider has a dispenser component instead
					Dispenser d = hit.transform.GetComponent<Dispenser>();
					// check is something is already snapped on the dispenser
					if (d != null) {
						// if not, create
						if (d.snappedHere == null) {
							d.DispenseObject();
						}
						// if yes, pick it up and empty the reference so new things can be spawned
						else {
							Pick(d.snappedHere);
							d.snappedHere = null;
						}
					}

				}
			}
			
			// if the player is holding something, check if the held thing can be snapped to a surface, otherwise just let it go
			else {

				// SphereCast to check if an ISnapSurface is nearby
				// SphereCast is described in the "if" above
				if (Physics.SphereCast(transform.position, 0.5f, transform.forward, out hit, 2f)) {

					// check if the hit object has a ISnapSurface component
					// this includes every component deriving from the ISnapSurface Interface
					ISnapSurface snapSurface = hit.transform.GetComponent<ISnapSurface>();
					if (snapSurface != null) {
						// if it does, snap the given object to said surface
						heldObject.transform.parent = null;
						snapSurface.Snap(heldObject);
					}
				}

				// if there is no ISnapSurface nearby, just let the item go
				else {
					Release();
				}
			}

		}

		// draws a ray in the scene view (to see if we're raycasting in the right direction and from the right place)
		Debug.DrawRay(transform.position, transform.forward, Color.blue);

	}

	// function to pick up an item
	// functions can have parameters, which are defined between the () brackets
	// parameters need a type ("Pickable" in this case) and a name ("pickable" in this case)
	// parameters can be referenced in a function by their name
	// whenever calling a function that requires parameters, we MUST parse a value for each of them
	void Pick(Pickable pickable) {

		// deactivate the physics on the parsed Pickable
		pickable.DeactivatePhysics();

		// make the Pickable's transform child of the pickupTarget transform to have it move with the player 
		pickable.transform.parent = pickupTarget;
		// move it to LOCAL 0,0,0 (Vector3.zero is a shortening for that) to center it
		pickable.transform.localPosition = Vector3.zero;

		// set the parsed Pickable as the current held object by assigning it to the heldObject variable
		heldObject = pickable;
	}

	// funtion to release the held item
	void Release() {
		// as the "heldObject" variable is of type Pickable, we can directly call the functions of the referenced Pickable instance
		// activate the held item phisics
		heldObject.ActivatePhysics();

		// unparent the heldObject transform from our player
		heldObject.transform.parent = null;

		// empty the heldObject variable
		heldObject = null;
	}

}