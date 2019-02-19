using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The abstract base class for all Card triggers.
// Subscribes the function inside the EffectInputBase to whatever event that this script represents. 
public abstract class TriggerBase : MonoBehaviour {

	// The script to run when the trigger has occured.
    [SerializeField] EffectInputBase effectInputScript;

	// Subscribes the function. Different depending on which trigger we're subscribing too.
	abstract protected void Subscribe();


	// Subscribes the function on Awake.
	private void Awake() {
		Subscribe();	
	}
}
