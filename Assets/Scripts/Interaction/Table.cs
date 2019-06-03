using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The "RequireComponent" attribute will automatically add the rwuired component together with this one
// A class can have more than one RequireComponent attribute
[RequireComponent(typeof(Collider))]
// When a class derives from one interface (or more), they are listed after the classes it derives from
// In this case, the "Dispenser" class derives from the "MonoBehaviour" class and from the "ISnapSurface" interface
public class Table : MonoBehaviour, ISnapSurface {

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
		}
		set {
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

	public Pickable UnSnap() {
		// create temp reference
		Pickable p = snappedHere;
		// empty local variable
		snappedHere = null;
		// return reference
		return p;
	}
	#endregion

}
