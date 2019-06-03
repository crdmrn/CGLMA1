using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
// this class is empty for now, and it's merely usable to distinguish a gameObject that can be picked
public class Pickable : MonoBehaviour {

	public Transform snapPoint;

	// public reference to the slider component to use for showing progression
	public Slider cookingSlider;

	// the amount of points needed for the Pickable to "cook" (lower = faster)
	public float cookingPoints = 100f;
	// private reference variable
	int _cookingPoint = 0;
	// get/set variable to store the item cooking progression
	public int cookingPoint {
		get {
			return _cookingPoint;
		} set {
			_cookingPoint = value;
			// increasde slider value to show the cooking progression
			if (cookingSlider != null)
				cookingSlider.value = _cookingPoint;
		}
	}

	// private reference variable
	Rigidbody _rigidbodyComponent;
	// public "getter" variable
	// "getter" variables don't store any data, they only define what happens when something tries to access them
	// that's why they USUALLY come with a "reference" variable
	// common practice wants "reference" variables to be named exactly like the "getters", but with a "_"
	public Rigidbody rigidbodyComponent {
		// when trying to get the value of this variable
		get {
			// check if the "reference" is empty or not
			// in case it's empty, try to populate it by getting the desired component
			if (_rigidbodyComponent == null)
				_rigidbodyComponent = GetComponent<Rigidbody>();
			// regardless if empty or not, return the "reference" value
			// a "getter" MUST include a return 
			return _rigidbodyComponent;
		}
	}

	Collider _colliderComponent;
	public Collider colliderComponent {
		get {
			if (_colliderComponent == null)
				_colliderComponent = GetComponent<Collider>();
			return _colliderComponent;
		}
	}

	Vector3 offset;
	void Start() {
		if (cookingSlider != null)
			offset = cookingSlider.transform.parent.localPosition;
	}

	void Update() {
		if (cookingSlider != null) {
			// keep slider in position
			cookingSlider.transform.parent.position = transform.position + offset;
			cookingSlider.transform.parent.rotation = Quaternion.identity;
		}
	}
	
	// Function to change all the necessary parameters to make sure the attached Rigidbody doesn't follow physics anymore
	public void DeactivatePhysics() {
		// set values for rigidbody so it doesn't follows physics
		rigidbodyComponent.isKinematic = true;
		rigidbodyComponent.useGravity = false;
		// set collider as trigger
		colliderComponent.isTrigger = true;
		// turn the renderer RED for debugging purposes
		/*Renderer r = GetComponent<Renderer>();
		if (r != null)
			r.material.color = Color.red;*/
	}

	// Function to change all the necessary parameters to make sure the attached Rigidbody starts folowing physics again
	public void ActivatePhysics() {
		// set values for rigidbody to start following physics
		rigidbodyComponent.isKinematic = false;
		rigidbodyComponent.useGravity = true;
		// set the collider back to solid
		colliderComponent.isTrigger = false;
		// turn the renderer GREEN for debugging purposes
		/*Renderer r = GetComponent<Renderer>();
		if (r != null)
			r.material.color = Color.green;*/
	}
}
