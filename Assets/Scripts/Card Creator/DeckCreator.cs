using System;
using System.Linq;
using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// Main Script, handles UI and input, as well as deck creation.
public class DeckCreator : MonoBehaviour {
	// ----------------------------------------- Fields and Properties ----------------------------------------- //	
	// The CSV Parser
	CSVParser Parser;

	// The canvas used for rendering the decks.
	[SerializeField] GameObject DeckRendererCanvas = null;

	// The decklist text input field and the deck's name.
	[SerializeField] Text DeckName = null;
	[SerializeField] Text DeckList = null;

	// The deck color picker options and the current color selected.
	[SerializeField] Image[] DeckColorOptions = null;
	int CurrentlySelectedColor = -1;


	// Container for all of the cards.
	[SerializeField] GameObject CardContainerPrefab = null;

	// A prefab to use for creating the cards of.
	[SerializeField] GameObject CardPrefab = null;

	
	
	// Singleton Instance
	public static DeckCreator Instance = null;



	// ----------------------------------------- Methods ----------------------------------------- //
	// --- Awake --- //
	void Awake() {
		// Make singleton
		if(Instance == null) {
			Instance = this;
		} else {
			Destroy(gameObject);
		}
	}

	// --- Start --- //
	void Start() {
		Parser = GetComponent<CSVParser>();
		OnDeckColorPress(0);
	}

	// ----------- Button Callbacks ----------- //
	// Makes a deck based on the input in the Deck List text component.
	public void OnMakeDeckFromDeckList() {
		List<Card> cards = new List<Card>();
		string deckList = DeckList.text;
		// Split list into rows.
		string[] rows = deckList.Split(new string[]{ "\n" }, StringSplitOptions.RemoveEmptyEntries);
		// Split each row into number/cardname pairs
		foreach(string row in rows) {
			string[] numberCardNamePair = row.Split(new char[]{ ' ' }, 2);
			int number = Convert.ToInt32(numberCardNamePair[0]);
			// Search the AllCards list for the card name
			Card card = Array.Find(Parser.AllCards, (c) => c.CardName == numberCardNamePair[1]);
			if(card.CardName != numberCardNamePair[1]) {
				Debug.LogWarning("Card not found in the Card Database! Card Name: " + numberCardNamePair[1]);
			}
			// Add to deck.
			for(int i = 0; i < number; i++) {
				cards.Add(card);
			}
		}
		// Make the deck.
		MakeDeck(cards.ToArray(), DeckName.text);
	}


	// Simply makes a deck with every single card in the database represented once.
	public void OnMakeAllCards() {
		MakeDeck(Parser.AllCards, "All Cards");
		print("Created all card gameobjects!");
	}


	// Callback for the deck color buttons
	public void OnDeckColorPress(int index) {
		CurrentlySelectedColor = index;
		for(int i = 0; i < DeckColorOptions.Length; i++) {
			if(CurrentlySelectedColor == i) {
				DeckColorOptions[i].GetComponentInChildren<CanvasGroup>().alpha = 1f;
			} else {
				DeckColorOptions[i].GetComponentInChildren<CanvasGroup>().alpha = 0f;
			}
		}
	}

	
	// ----------- Helper Functions ----------- //
	// Creates a gameobject with several card prefab objects as children, with each card in the given cards list inside of it.
	// From Card Struct -> Card GameObject
	void MakeDeck(Card[] deck, string deckName) {
		if(deck.Length == 0) {
			Debug.LogWarning("No Cards in the deck list!");
			return;
		}

		// Delete all children under the deck renderer canvas.
		foreach(Transform child in DeckRendererCanvas.transform) {
			Destroy(child.gameObject);
		}

		// Create parent, rename parent so it has the date.
		GameObject CardContainer = Instantiate(CardContainerPrefab, DeckRendererCanvas.transform);
		CardContainer.name = deckName + " - " + DateTime.Now;
		
		// Create an object for each card using the card prefab template.
		for(int i = 0; i < deck.Length; i++) {
			Card card = deck[i];
			CardScript cardScript = Instantiate(CardPrefab, CardContainer.transform).GetComponent<CardScript>();
			cardScript.gameObject.name = card.CardName;
			Utility.Instance.ApplyCardInfoToCardObject(card, cardScript);
		}
	}
	

	
}
