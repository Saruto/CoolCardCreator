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
	[SerializeField] GameObject DeckRendererCanvas;

	// The decklist text input field and the deck's name.
	[SerializeField] Text DeckName;
	[SerializeField] Text DeckList;

	// The deck color picker options and the current color selected.
	[SerializeField] Image[] DeckColorOptions;
	int CurrentlySelectedColor = -1;


	// Container for all of the cards.
	[SerializeField] GameObject CardContainerPrefab;

	// A prefab to use for creating the cards of.
	[SerializeField] GameObject CardPrefab;

	// Prefab for a mana symbol
	[SerializeField] GameObject ManaSymbolPrefab;

	// Prefabs for every mana symbol sprite
	[SerializeField] Sprite PurpleSymbol;
	[SerializeField] Sprite RedSymbol;
	[SerializeField] Sprite GraySymbol;
	[SerializeField] Sprite GreenSymbol;
	[SerializeField] Sprite YellowSymbol;
	[SerializeField] Sprite NeutralSymbol;
	// Other Sprites
	[SerializeField] Sprite TransparentSprite;



	// ----------------------------------------- Methods ----------------------------------------- //
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
			Card card = Parser.AllCards.Find((c) => c.CardName == numberCardNamePair[1]);
			if(card.CardName != numberCardNamePair[1]) {
				Debug.LogWarning("Card not found in the Card Database! Card Name: " + numberCardNamePair[1]);
			}
			// Add to deck.
			for(int i = 0; i < number; i++) {
				cards.Add(card);
			}
		}
		// Make the deck.
		MakeDeck(cards, DeckName.text);
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
	void MakeDeck(List<Card> deck, string deckName) {
		if(deck.Count == 0) {
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
		
		// Create a prefab for each card using the card prefab template.
		for(int i = 0; i < deck.Count; i++) {
			Card card = deck[i];
			CardInfo cardScript = Instantiate(CardPrefab, CardContainer.transform).GetComponent<CardInfo>();
			cardScript.gameObject.name = card.CardName;
			// Set fields
			cardScript.Background.color = DeckColorOptions[CurrentlySelectedColor].color;
			cardScript.CardName.text = card.CardName;
			cardScript.CardText.text = card.CardText;
			// Mana
			IEnumerable<char> manaSymbols = card.ManaCost.ToCharArray().Reverse();
			foreach(char symbol in manaSymbols) {
				GameObject symbolIcon = Instantiate(ManaSymbolPrefab, cardScript.ManaCostLayout.transform);
				switch(symbol) {
				case 'P': symbolIcon.GetComponent<Image>().sprite = PurpleSymbol; break;
				case 'R': symbolIcon.GetComponent<Image>().sprite = RedSymbol; break;
				case 'A': symbolIcon.GetComponent<Image>().sprite = GraySymbol; break;
				case 'G': symbolIcon.GetComponent<Image>().sprite = GreenSymbol; break;
				case 'Y': symbolIcon.GetComponent<Image>().sprite = YellowSymbol; break;
				default: 
					symbolIcon.GetComponent<Image>().sprite = NeutralSymbol;
					symbolIcon.GetComponentInChildren<Text>().text = symbol.ToString();
					break;
				}
			}
			// Attack/Health
			if(card.Attack != -1) {
				cardScript.AttackText.text = card.Attack.ToString();
			} else {
				cardScript.AttackText.text = string.Empty;
				cardScript.AttackIcon.sprite = TransparentSprite;
			}
			if(card.Health != -1) {
				cardScript.HealthText.text = card.Health.ToString();
			} else {
				cardScript.HealthText.text = string.Empty;
				cardScript.HealthIcon.sprite = TransparentSprite;
			}
			// Directions
			if(card.Directions.HasFlag(Directions.None)) {
				cardScript.UpArrow.enabled = false;
				cardScript.DownArrow.enabled = false;
				cardScript.LeftArrow.enabled = false;
				cardScript.RightArrow.enabled = false;
			} else {
				if(card.Directions.HasFlag(Directions.Up)) {
					cardScript.UpArrow.enabled = true;
				}
				if(card.Directions.HasFlag(Directions.Down)) {
					cardScript.DownArrow.enabled = true;
				}
				if(card.Directions.HasFlag(Directions.Left)) {
					cardScript.LeftArrow.enabled = true;
				}
				if(card.Directions.HasFlag(Directions.Right)) {
					cardScript.RightArrow.enabled = true;
				}
			}
		}
	}
}
