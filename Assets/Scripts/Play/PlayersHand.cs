using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Script that handles behavior that goes on with the player's hand, like the cards in the hand
// getting closer together when there's more of them, etc.
public class PlayersHand : MonoBehaviour {
	// --- Fields and Properties --- //

	// The Radial Layout
	RadialLayout layoutGroup;
	

	// Seralized Fields
	[SerializeField] GameObject CardInHandTemplate = null;

	
	// --- Methods --- //
    // Start
    void Start() {
        layoutGroup = GetComponent<RadialLayout>();
		// Fills the players hand with random cards.
		for(int i = 0; i < 7; i++) {
			GameObject card = Instantiate(CardInHandTemplate, transform);
			card.GetComponent<CardScript>().UpdateCardVisuals(CardDatabase.Instance.AllCards[61]);
		}
    }


    // Update
    void Update() {
		
	}
}
