// LIBRARIES to be used in this script
// libraries are created like the namespace in the comment below
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

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

		public StudioEventEmitter footstepsEmitter;
		public StudioParameterTrigger footstepSpeedParameter;

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

		// store reference to player's Animator component
		Animator playerAnimator;

		Vector3 planarVelocity {
			get {
				Vector3 temp = playerRigidbody.velocity;
				temp.y = transform.position.y;
				return temp;
			}
		}

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

			// look for an Animator Component among the children of the player gameobject
			playerAnimator = GetComponentInChildren<Animator>();

			StartCoroutine(Footsteps());
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
			Vector3 direction = new Vector3(input.x, 0f, input.y) * acceleration;
			//direction.y = transform.position.y;

			// apply consequent force to Rigidbody trough the AddForce(Vector3 forceMode, ForceMode forceMode) method of the Rigidbody class
			// between the brackets we pass the parameters required by the method
			playerRigidbody.AddForce(direction, ForceMode.VelocityChange);

			// now we need to cap the speed of our character, as otherwise it will accelerate infinitely as long as we keep a button pressed

			// store velocity of the rigidbody (that includes the direction, as it's a vector)
			Vector3 rVelocity = playerRigidbody.velocity;

			// if the magnitude (length) of the velocity is bigger of the set max speed
			if (planarVelocity.magnitude > maxSpeed) {

				Vector3 clampedVelocity = Vector3.ClampMagnitude(planarVelocity, maxSpeed);
				// using the Vector3.ClampMagnitude(Vector3 vector, float MaxLength) method, clamp the velocity Vector3 of the player Rigidbody component thus clamping it's movement speed
				playerRigidbody.velocity = new Vector3(clampedVelocity.x, playerRigidbody.velocity.y, clampedVelocity.z); // Vector3.ClampMagnitude(playerRigidbody.velocity, maxSpeed);

				footstepsEmitter.SetParameter("Speed", playerRigidbody.velocity.magnitude / maxSpeed);
			}


			// write the velocity value to the console to check if our code works
			//Debug.Log(playerRigidbody.velocity.magnitude);

			// ---- LESSON 2 ----

			// to allow the Animator component to blend between IDLE, WALK and RUN animations, we need to feed him the speed at which the player is moving
			// first, we make sure the Animator component is actually attached to our player gameObject
			if (playerAnimator != null) {
				// if it is we use the Animator.SetFloat(string name, float value) to set the value of the Speed variable in the AnimatorController used by the Animator
				playerAnimator.SetFloat("Speed", playerRigidbody.velocity.magnitude);
			}

			// when the player is moving, we want it to face the movement direction
			// to check if the player is moving, we just chef if velocity.sqrMgnitude value of its rigidbody is bigger than 0
			// Vector3.sqrMagnitude (squared Magnitude) is quicker to access than Vector3.Magnitude
			if (playerRigidbody.velocity.sqrMagnitude > 0.001f) {
				// we call the function to rotate the player
				//such function is defined some lines below, in the "CharacterRotation" #region
				RotateCharacter();
			}

		}

		private void LateUpdate() {
			if (Input.GetButtonDown("Jump"))
				Jump();
		}

		// #region and #endregion can be used to better organize the code
		#region CharacterRotation

		// float variable to allow us to decide how fast should the plazer rotate towards the movement direction
		public float rotationSpeed = 1f;

		// function to rotate the character
		// void means that this function DOES NOT return any value, it only runs some code
		// the empty brackets () next to the function name mean that this function does not need any values to be paresd in order to work
		void RotateCharacter() {
			// get the rotation of the playerRigidbody velocity vector
			// Quaternions are used to sore rotation data, they work like Vectors do for directions
			Vector3 planarDirection = new Vector3(playerRigidbody.velocity.x, 0f, playerRigidbody.velocity.z);
			//Debug.Log(planarDirection.sqrMagnitude);
			if (planarDirection.sqrMagnitude < 0.01f)
				return;

			Quaternion directionRot = Quaternion.LookRotation(planarDirection);

			// now that we know which is the rotation we need to apply to our character in  order to match the movement direction
			// we want to make sure that it?s not applied all at once, but instead only part of it every frame, to obtain a lerping effect
			// Quaterion.Slerp(Quaternion a, Quaternion b, float t) returns the interpolated t value between a and b, where t is clamped between 0 (which returns a) and 1 (which returns b)
			// Time.deltaTime is a float indicating the amount of time in seconds that it took the computer to render the last frame
			// by multiplying the rotation speed with Time.deltaTime, we make it independent of the framerate
			Quaternion lerpedRot = Quaternion.Slerp(playerRigidbody.rotation, directionRot, Time.deltaTime * rotationSpeed);

			// after we calculated the desired rotation for this frame, we apply it to the rigidbody by using its MoveRotation(Quaternion rotation) function
			playerRigidbody.MoveRotation(lerpedRot);
		}
		#endregion

		public LayerMask raycastLayer;

		Dictionary<Vector3, bool> jumpPositions = new Dictionary<Vector3, bool>();
		public float jumpForce = 5f;
		public int airborneJumps = 1;
		int currentAirborneJumps = 0;
		bool isGrounded {
			get {
				Ray ray = new Ray(transform.position + (Vector3.one * 0.1f), Vector3.down);
				Debug.DrawRay(transform.position + (Vector3.one * 0.1f), Vector3.down * 0.2f, Color.red, 10f);
				return Physics.Raycast(ray, 0.2f, raycastLayer);
			}
		}

		public Transform wallJumpChecker;
		bool canWallJump {
			get {
				Ray ray = new Ray(wallJumpChecker.position, wallJumpChecker.forward);
				Debug.DrawRay(ray.origin, ray.direction, Color.blue, 10f);
				return Physics.Raycast(ray, 1f, raycastLayer);
			}
		}

		void Jump() {
			if (isGrounded) {
				playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
				currentAirborneJumps = 0;
				if (!jumpPositions.ContainsKey(transform.position))
					jumpPositions.Add(transform.position, false);
			} else if (!isGrounded && canWallJump) {
					Vector3 direction = Vector3.up * jumpForce;
					direction.x = (transform.right - transform.position).z * 10f;
					playerRigidbody.AddForce(Vector3.up * jumpForce * 2f, ForceMode.Impulse);
					playerRigidbody.AddForce(Vector3.right * 10f * (transform.right - transform.position).z, ForceMode.VelocityChange);
					Debug.DrawRay(transform.position, direction, Color.green, 10f);
					//playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
			} else if (currentAirborneJumps < airborneJumps) {
				playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
				currentAirborneJumps++;
				if (!jumpPositions.ContainsKey(transform.position))
					jumpPositions.Add(transform.position, true);
			}
		}

		public float footstepThreshold = 1f;

		public void Footstep() {
			AnimationEvent e;
			//playerAnimator.
					//	footstepsEmitter.Play();
		}

		IEnumerator Footsteps() {
			float step = 0f;
			while (true) {
				//playerAnimator.fireEvents
				/*if (playerRigidbody.velocity.sqrMagnitude > 0f) {
					//footstepsEmitter.SetParameter("Speed", playerRigidbody.velocity.magnitude / maxSpeed);
					if (!footstepsEmitter.IsPlaying())
				}
				else {
					//footstepsEmitter.SetParameter("Speed", 0f);
					if(footstepsEmitter.IsPlaying())
						footstepsEmitter.Stop();
				}*/
				/*if (playerRigidbody.velocity.sqrMagnitude > 0f) {
					Debug.Log(playerRigidbody.velocity.sqrMagnitude);
					step += 0.1f * playerRigidbody.velocity.sqrMagnitude;
					if (step >= footstepThreshold) {
						if (footstepsEmitter != null)
							footstepsEmitter.Play();
						step = 0f;
					}
				} else {
					step = 0f;
				}*/
				yield return null;
			}
		}

		private void OnDrawGizmos() {
			foreach(KeyValuePair<Vector3, bool> entry in jumpPositions) {
				Color backup = Gizmos.color;
				Gizmos.color = entry.Value ? Color.blue : Color.red;
				Gizmos.DrawSphere(entry.Key, 0.5f);
				Gizmos.color = backup;
			}
		}

	}

}