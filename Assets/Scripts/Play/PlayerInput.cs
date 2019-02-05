using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		bool didHit = Physics.Raycast(ray, out hit);
		CardScript cardScript = didHit ? hit.transform.GetComponentInChildren<CardScript>() : null;
		if(didHit && cardScript != null) {
			HighlightDisplay.gameObject.SetActive(true);
			HighlightDisplay.UpdateCardVisuals(cardScript.card);
		} else {
			HighlightDisplay.gameObject.SetActive(false);
		}
	}
}
