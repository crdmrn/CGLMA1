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

		Rigidbody r;

		// Use this for initialization
		void Start () {
			// access rigidbody on the same gameObject
			r = GetComponent<Rigidbody>();
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
			r.AddForce(input * 10f, ForceMode.Acceleration);
			// write value to console
			Debug.Log(input);
		}

	}

}