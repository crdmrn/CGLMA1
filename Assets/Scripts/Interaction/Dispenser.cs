using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The "RequireComponent" attribute will automatically add the rwuired component together with this one
// A class can have more than one RequireComponent attribute
[RequireComponent(typeof(Collider))]
// When a class derives from one interface (or more), they are listed after the classes it derives from
// In this case, the "Dispenser" class derives from the "MonoBehaviour" class and from the "ISnapSurface" interface
public class Dispenser : MonoBehaviour, ISnapSurface {

	// When a class derives from an Interface, it MUST implement all of the Interface's variables and functions
	// When implementing said variables and functions, they MUST be marked as public
	// It is a good practice to group said variables and funtions in a #region
	#region ISnapSurface implementation
	[SerializeField]
	Transform _snapPoint;
	public Transform snapPoint {
		get {
			return _snapPoint;
		}
	}

	Pickable _snappedHere;
	public Pickable snappedHere {
		get {
			return _snappedHere;
		} set {
			_snappedHere = value;
		}
	}

	// Interfaces do not define the content of the functions, this can be different in each class that implements said Interface
	// See other implementations of the ISnapSurface interface to have a better grasp of this
	public void Snap(Pickable pickable) {
		pickable.DeactivatePhysics();
		pickable.transform.position = snapPoint.position - pickable.snapPoint.localPosition;
		snappedHere = pickable;
	}
	#endregion

	// Reference variable to the prefab we want to be spawned
	public GameObject dispensedObject;

	// The function to call when we want to spawn ("dispense") an instance of the dispencedObject
	public void DispenseObject() {
		// If nothing is snapped to this surface
		if (snappedHere == null) {
			// Instantiate the dispencedObject and snap it to this surface
			GameObject GO = (GameObject)Instantiate(dispensedObject);
			Pickable p = GO.GetComponent<Pickable>();
			Snap(p);
		}
	}

	// OnDrawGizmosSelected is a built-in Unity function that allows us to draw stuff in the Editor Scene view for debugging purposes
	// As the name suggests, those things are only drawn when the gameObject that has this component is selected in the Hierarchy
	// To have things drawn regardless of the Hierarchy selection, use OndrawGizmosInstead
	// Things drawn in the Scene view are NOT visible in the Game view nor in the builds
	private void OnDrawGizmosSelected() {
		if (snapPoint != null) {
			if (snappedHere == null)
				Gizmos.color = Color.red;
			else
				Gizmos.color = Color.green;
			Gizmos.DrawSphere(snapPoint.position, 0.3f);
		}
	}

}