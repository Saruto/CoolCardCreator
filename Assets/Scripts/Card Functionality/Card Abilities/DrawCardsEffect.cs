using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCardsEffect : EffectBase {
	[SerializeField] int numCardsToDraw = 0;

	// Draws 2 cards.
	public override void onDoThing() {
		print("Player draws " + numCardsToDraw + " cards!");
	}
}
