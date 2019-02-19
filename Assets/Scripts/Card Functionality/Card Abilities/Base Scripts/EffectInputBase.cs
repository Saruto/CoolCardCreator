using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The function signurature for card effects and inputs.
public delegate void CardEffectHandler();


public abstract class EffectInputBase : MonoBehaviour {
	// The effect or input to trigger for this specific effect/input script.
    public abstract void onDoThing();

}
