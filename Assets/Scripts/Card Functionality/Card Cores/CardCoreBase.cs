using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A base class representing the core part and behavior of a card.
public abstract class CardCoreBase : MonoBehaviour {
	// The behavior of the card when it's played by the player.
	public abstract void OnPlayed();
}
