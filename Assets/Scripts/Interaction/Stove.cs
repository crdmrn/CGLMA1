using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The "RequireComponent" attribute will automatically add the rwuired component together with this one
// A class can have more than one RequireComponent attribute
[RequireComponent(typeof(Collider))]
// When a class derives from one interface (or more), they are listed after the classes it derives from
// In this case, the "Dispenser" class derives from the "MonoBehaviour" class and from the "ISnapSurface" interface
public class Stove : MonoBehaviour, ISnapSurface {

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

	// private reference variable
	Pickable _snappedHere;
	// get/set variable to store the reference to the snapped object
	// is set up in a way that as soon as something is snapped here, a cook coroutine is started
	// when it gets emptied (= the corrently snapped object is removed) the coroutine is stopped
	public Pickable snappedHere {
		get {
			return _snappedHere;
		}
		set {
			Pickable oldSnapped = _snappedHere;
			// set the value of the reference variable
			_snappedHere = value;
			// start/stop cooking coroutine depending on said variable
			if (_snappedHere != null) {
				if (_snappedHere.cookingSlider != null)
					_snappedHere.cookingSlider.gameObject.SetActive(true);
				cookCoroutine = StartCoroutine(Cook());
				Debug.Log("COOK");
			} else {
				if (cookCoroutine != null) {
					if (oldSnapped.cookingSlider != null)
						oldSnapped.cookingSlider.gameObject.SetActive(false);
					StopCoroutine(cookCoroutine);
					cookCoroutine = null;
					Debug.Log("STOP COOKING");
				}
			}
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

	public float cookingTime = 5f;

	// private variable to store reference to the Coroutine used to currently "cook" the snapped object
	// storing reference to a Coroutine allows th specifically stop that coroutine
	// it also allows us to make sure only one coroutine of that kind is running
	Coroutine cookCoroutine;
	// cook Coroutine
	IEnumerator Cook() {
		while (snappedHere.cookingPoint < snappedHere.cookingPoints) {
			// wait for the amount of time needed to make sure the overall cooking time is respected
			yield return new WaitForSeconds(cookingTime / snappedHere.cookingPoints);
			// ++ increases int variables by 1 (-- decreases it)
			snappedHere.cookingPoint++;
		}
	}
	#endregion

}
