using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
// this class is empty for now, and it's merely usable to distinguish a gameObject that can be picked
public class Pickable : MonoBehaviour {

	public Transform snapPoint;

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
	
	// Function to change all the necessary parameters to make sure the attached Rigidbody doesn't follow physics anymore
	public void DeactivatePhysics() {
		// set values for rigidbody so it doesn't follows physics
		rigidbodyComponent.isKinematic = true;
		rigidbodyComponent.useGravity = false;

		// set collider as trigger
		colliderComponent.isTrigger = true;

		// turn the renderer RED for debugging purposes
		Renderer r = GetComponent<Renderer>();
		if (r != null)
			r.material.color = Color.red;
	}

	// Function to change all the necessary parameters to make sure the attached Rigidbody starts folowing physics again
	public void ActivatePhysics() {
		// set values for rigidbody to start following physics
		rigidbodyComponent.isKinematic = false;
		rigidbodyComponent.useGravity = true;

		// set the collider back to solid
		colliderComponent.isTrigger = false;

		// turn the renderer GREEN for debugging purposes
		Renderer r = GetComponent<Renderer>();
		if (r != null)
			r.material.color = Color.green;
	}
}
