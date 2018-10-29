// libraries
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overcooked {

	[RequireComponent(typeof(Rigidbody))]
	public class Movement : MonoBehaviour {

		//booleans can be false/true
		bool testBool;

		// whole number (1 2 3 ... )
		int integer;
		
		// number with decimals (1,2 0,005) up to 7 digits, no matter the amount of decimals
		float nuberwithdecimals;

		// like a float but double the size (more heavy)
		double doublesizeafloat;

		// a single character
		char letter;

		// an array of characters
		string word;

		// store input values from keyboard/controller
		Vector3 input;

		// store reference to player Rigidbody component
		Rigidbody playerRigidbody;

		public float acceleration;
		public float maxSpeed;

		// Use this for initialization
		void Start () {
			// access rigidbody on the same gameObject
			playerRigidbody = GetComponent<Rigidbody>();
		}
	
		// Update is called once per frame
		void Update () {
			// store the value of "Horizontal" axis
			float inputH = Input.GetAxis("Horizontal");
			// store the value of "Vertical" axis
			float inputV = Input.GetAxis("Vertical");
			// combine them into a Vector3 (float x, float y, float z);
			input = new Vector3(inputH, 0f, inputV);
			// GetComponent is a very heavy function, better not call it in the Update
			//Rigidbody r = GetComponent<Rigidbody>();
			// apply consequent force to rigidbody
			playerRigidbody.AddForce(input * acceleration, ForceMode.VelocityChange);

			// store velocity of the rigidbody (that includes the direction, as it's a vector)
			Vector3 rVelocity = playerRigidbody.velocity;
			// if the magnitude (length) of the velocity is bigger of the set max
			if (playerRigidbody.velocity.magnitude > maxSpeed)
			{
				// take normalized (=1) form of the velocity and multiply it by the max speed
				Vector3 cappedVelocity = rVelocity.normalized * maxSpeed;
				// apply it to the rigidbody
				playerRigidbody.velocity = cappedVelocity;
			}
			Debug.Log(playerRigidbody.velocity.magnitude);

			// write value to console
			//Debug.Log(input);
		}

	}

}