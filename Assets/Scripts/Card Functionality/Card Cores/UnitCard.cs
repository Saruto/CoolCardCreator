using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Represents Unit Cards
public class UnitCard : CardCoreBase {
	// The card's power and toughness.
	[SerializeField] public int Power;
	[SerializeField] public int Toughness;


	// The events that run for this card.
	public event CardEffectHandler OnBattlecry;


	// The behavior of the card when it's played by the player.
	public override void OnPlayed() {
		// Trigger Battlecries //
		OnBattlecry?.Invoke();
	}
}
