using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FootstepSounds : MonoBehaviour
{

	public StudioEventEmitter footstepSounds;
    public void Footstep() {
			footstepSounds.Play();
	}
}
