using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Represents Unit Cards
public class UnitCard : CardCoreBase {
	// The card's power and toughness.
	[SerializeField] public int Power;
	[SerializeField] public int Toughness;

	// The behavior of the card when it's played by the player.
	public override void OnPlayed() {
		throw new NotImplementedException();
	}
}
