using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Script that handles behavior that goes on with the player's hand, like the cards in the hand
// getting closer together when there's more of them, etc.
public class PlayersHand : MonoBehaviour {
	// --- Fields and Properties --- //

	// The horizontal layout group component attached to this GO
	HorizontalLayoutGroup hLayoutGroup;
	

	// Seralized Fields
	[SerializeField] 

	
	// --- Methods --- //
    // Start
    void Start() {
        hLayoutGroup = GetComponent<HorizontalLayoutGroup>();
    }


    // Update
    void Update() {
        // TODO: Should only need to do this once we detect a change in the hand size.
		RespaceCards();
    }

	
	
	// Respaces the cards in hand based on the number of them in the hand.
	void RespaceCards() {
		int numChildren = transform.childCount;
		 

		// update layout
		//LayoutRebuilder.MarkLayoutForRebuild(GetComponent<RectTransform>());
	}
}
