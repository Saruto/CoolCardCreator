using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// A high level script that manages some player input functions.
// Also provides services to component scripts in the scene, allowing them to interact with other components.
public class PlayerInput : MonoBehaviour {
	// --- Fields and Properties --- //


	// Seralized Fields
	// The zoomed in card. 
	[SerializeField] CardScript HighlightDisplay;


	// --- Methods --- //
	// Update
	void Update() {
		// -- Zoom in of the highlighted card -- //
		CheckForHoveredCard();

	}


	// Raycasts on both the physical world and the UI layer to see if the player is hovering over a card.
	// If they are, show the zoomed in card with that card info on it.
	void CheckForHoveredCard() {
		// Cards on table
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		bool didHit = Physics.Raycast(ray, out hit);
		CardScript cardScript = didHit ? hit.transform.GetComponentInChildren<CardScript>() : null;
		if(didHit && cardScript != null) {
			HighlightDisplay.gameObject.SetActive(true);
			HighlightDisplay.UpdateCardVisuals(cardScript.card);
			return;
		}
		// Cards in hand
        List<RaycastResult> results = new List<RaycastResult>();
		PointerEventData ped = new PointerEventData(EventSystem.current);
		ped.position = Input.mousePosition;
        GetComponent<GraphicRaycaster>().Raycast(ped, results);
        foreach (RaycastResult result in results){
			CardScript resultCS = result.gameObject.GetComponentInChildren<CardScript>();
			if(resultCS != null) {
				HighlightDisplay.gameObject.SetActive(true);
				HighlightDisplay.UpdateCardVisuals(resultCS.card);
				return;
			}
        }
		HighlightDisplay.gameObject.SetActive(false);
	}
}
