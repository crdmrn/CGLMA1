// LIBRARIES to be used in this script
// libraries are created like the namespace in the comment below
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NAMESPACES are used to separate code into chunks
// things inside a namespace tag are available to other (in different files as well) scripts that are included in the same namespace tag
// they can be added to a script via the "using" tag (see libraries above), this works only in one direction
namespace Overcooked {

	// Unity-specific attribute, that automatically adds the componet inside the typeof brackets to the prefab or gameObject
	// when the class below is added to it
	// IT ONLY WORKS IN THE EDITOR, AND NOT WHILE IN PLAYMODE
	[RequireComponent(typeof(Rigidbody))]
	// a CLASS is a code template consisting of variables and methods (sometimes referred to as functions)
	// a class can derive from another class, such class can be specified after the ":"
	// most of the classes in Unity derive from MonoBehaviour, which is the one responsible for calling built-in functions such as "Start" or "Update" on such classes
	public class Movement : MonoBehaviour {

		// variables are used to store data
		// oversemplifying, for now, there re 6 main kind of variables in c#
		// variables can be public (available to other classes as well) or private (usable only within the same class)
		// in the Unity editor, public variables are visible in the inspector panel

		// booleans can be false/true
		bool boolean;

		// integers are whole numbers, positive or negative (-1, 0, 1, 2, 3 ... )
		// int variables are 32bit long, this means that they can store values from -2.147.483.648 (-2^31) to 2.147.483.647 (2^31)
		int integer;

		// float are number with decimals (1,2 0,005) up to 7 digits, no matter the amount of decimals
		// this means that if I want to have a number with 7 digits BEFORE the decimals (e.g. 1.000.000) I won't be able to add decimals
		// float variables are 32bit long and can store values between (1,5 × 10^-45) and (3,4 × 10^38)
		float nuberWithDecimals;

		// dounbles are like floats but have double the bit size (more heavy)
		// double variables are 64bit long and can store values between (5,0 × 10^−324) and  (1,7 × 10^308)
		double doubleSizeFloat;

		// a single unicode character
		char letter;

		// an array of characters
		string word;

		// to move our characters we'll need some variables

		// store input values from keyboard/controller
		// a Vector2 is a class to store data of a 2-dimensional Vector
		Vector2 input;

		// store the reference to the Rigidbody component attached to our player gameObject
		Rigidbody playerRigidbody;

		// allow us to decide the acceleration of our player
		// we set it to public so we can experiment in the editor without changing the value in the code
		public float acceleration;

		// allow us to decide the maximum speed our player will move at
		// we set it to public for the same reason of the variable above
		public float maxSpeed;

		// Start is called only once, the FIRST time the gameObject this component is attached to gets enabled
		void Start () {
			// GetComponent<T> checks if a Rigidbody component is attached to this gameObject and eventully returns its reference
			// we store such reference in a variable, so we can access it whenever we want, as THE GETCOMPONENT FUNCTION IS VERY HEAVY TO PERFORM
			playerRigidbody = GetComponent<Rigidbody>();
		}

		// Update is called once per frame
		void Update() {

			//first, we need to read the input from the keyboard/ controller 

			// store the value of "Horizontal" axis, using the Input class
			float inputH = Input.GetAxis("Horizontal");
			// do the same for the value of "Vertical" axis
			float inputV = Input.GetAxis("Vertical");

			// combine them into our Vector2 variable (float x, float y)
			// the "new" constructor allows me to create new instances of a class, in this case Vector2
			// if the class allows for it, parameters can be passed to the constructor between the brackets
			input = new Vector2(inputH, inputV);
		}

		// to apply the required force to our player Rigidbody to move it around we use the Mnobehavour.FixedUpdate method
		// FixedUpdate gets called every fixed TimeStep
		// TimeStep can be set at Edit/ProjectSettings/Time and by default is 0.02 seconds
		// using FixedUpdate to apply physics forces, allows us to detach their effect from the game's framerate
		private void FixedUpdate() {

			// as our player will move on the xz plane, we need to translate our Vector2 input into a Vector3 (3D)
			Vector3 direction = new Vector3(input.x, 0f, input.y);

			// apply consequent force to Rigidbody trough the AddForce(Vector3 forceMode, ForceMode forceMode) method of the Rigidbody class
			// between the brackets we pass the parameters required by the method
			playerRigidbody.AddForce(direction * acceleration, ForceMode.VelocityChange);

			// now we need to cap the speed of our character, as otherwise it will accelerate infinitely as long as we keep a button pressed

			// store velocity of the rigidbody (that includes the direction, as it's a vector)
			Vector3 rVelocity = playerRigidbody.velocity;

			// if the magnitude (length) of the velocity is bigger of the set max speed
			if (rVelocity.magnitude > maxSpeed) {

				// using the Vector3.ClampMagnitude(Vector3 vector, float MaxLength) method, clamp the velocity Vector3 of the player Rigidbody component thus clamping it's movement speed
				playerRigidbody.velocity = Vector3.ClampMagnitude(playerRigidbody.velocity, maxSpeed);

			}

			// write the velocity value to the console to check if our code works
			Debug.Log(playerRigidbody.velocity.magnitude);
		}

	}

}