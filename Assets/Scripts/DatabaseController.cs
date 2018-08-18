using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Represents a single row of card information from the CSV
public struct Card {
	public int ID;
	public string CardName;
	public int ManaCost;
	public string Type;
	public string CardText;
	public int Attack;
	public int Health;
	public Directions Directions;
	public Rarity Rarity;
}

// Enum flag, used to keep track of attack directions.
// For example: "Directions dir = Directions.Up | Directions.Left;" represents a unit with the up and left attack directions.
// dir.HasFlag(Directions.Up) function can be used to check if an enum has a flag set. 
[Flags]
public enum Directions { None = 0, Up = 1, Down = 2, Left = 4, Right = 8 }

// Used to keep track of Rarity.
public enum Rarity { Common, Uncommon, Rare, Mythic }


// Main Script
public class DatabaseController : MonoBehaviour {
	// ----------------------------------------- Fields and Properties ----------------------------------------- //
	// All of the cards parsed from the CSV
	List<Card> AllCards = new List<Card>();
	

	// The canvas used for rendering the decks.
	[SerializeField] GameObject DeckRendererCanvas;

	// The camera that's currently rendering the deck.
	[SerializeField] Camera DeckRendererCamera;

	// Container for all of the cards.
	[SerializeField] GameObject CardContainerPrefab;

	// A prefab to use for creating the cards of.
	[SerializeField] GameObject CardPrefab;


	// ----------------------------------------- Methods ----------------------------------------- //
	// ----------- Button Callbacks ----------- //
	// Parses entire CSV and adds it to the AllCards list.
	public void OnParseCSV() {
		// Clear the list.
		AllCards.Clear();

		// Get CSV, strip the first row.
		string NewlineCharacter = Environment.NewLine;
		TextAsset csv = (TextAsset)Resources.Load("Card Database");
		string csvText = csv.text.Substring(csv.text.IndexOf(NewlineCharacter) + NewlineCharacter.Length);

		// Split the text into rows, using NewlineCharacter as the demimiter.
		string[] rows = csvText.Split(new string[]{ NewlineCharacter }, StringSplitOptions.None);

		// Usually the .csv file will have an empty row at the end, so we throw this out.
		rows = rows.Take(rows.Length - 1).ToArray();

		// For each row, split them up by commas, giving us each element in the row, and then create the card and assign them to their associated variables.
		foreach(string row in rows) {
			string[] elements = row.Split(',');
			// Need to do this to strip out random newline characters.
			elements = elements.Select((element) => { return Regex.Replace(element, @"\t|\n|\r", ""); }).ToArray();

			// Assign variables.
			Card card = new Card();
			card.ID = Convert.ToInt32(elements[0]);
			card.CardName = elements[1];
			card.ManaCost = Convert.ToInt32(elements[2]);
			card.Type = elements[3];
			card.CardText = elements[4];
			if(elements[5] != "") {
				card.Attack = Convert.ToInt32(elements[5]);
			} else {
				card.Attack = -1;
			}
			if(elements[6] != "") {
				card.Health = Convert.ToInt32(elements[6]);
			} else {
				card.Health = -1;
			}
			Enum.TryParse(elements[8], out card.Rarity);
			
			// Parse Directions
			string[] csvDirections = elements[7].Split('/');
			Directions directions = Directions.None;
			foreach(string dir in csvDirections) {
				switch(dir) {
				case "u":
					directions |= Directions.Up; break;
				case "d":
					directions |= Directions.Down; break;
				case "l":
					directions |= Directions.Left; break;
				case "r":
					directions |= Directions.Right; break;
				}
			}
			card.Directions = directions;
			
			// Add the card to the cards list
			AllCards.Add(card);
		} 
		
		print("All cards successfully parsed! Number of Cards: " + AllCards.Count);
	}


	// Simply makes a deck with every single card in the database represented once.
	public void OnMakeAllCards() {
		MakeDeck(AllCards, "All Cards");
		print("Created all card gameobjects!");
	}

	
	// Saves the deck that is currently being rendered by the DeckRendererCamera.
	public void OnSaveDeck() {
		string PATH = Application.persistentDataPath + "/Deck - " + DateTime.Now.ToString("MM-dd-yyyy-HH-mm-ss") + ".png";
		// Create texture from the DeckRendererCamera's target texture.
		Texture2D texture = new Texture2D(4000, 2800, TextureFormat.ARGB32, false);
		RenderTexture.active = DeckRendererCamera.targetTexture;
		texture.ReadPixels(new Rect(0, 0, 4000, 2800), 0, 0);
		texture.Apply();
		RenderTexture.active = null;
		// Encode texture to png and save it.
		byte[] bytes = texture.EncodeToPNG();
		System.IO.File.WriteAllBytes(PATH, bytes);
		print("Deck image creation successful! Saved to: " + PATH);
	}

	
	// ----------- Helper Functions ----------- //
	// Creates a gameobject with several card prefab objects as children, with each card in the given cards list inside of it.
	void MakeDeck(List<Card> deck, string deckName) {
		if(deck.Count == 0) {
			Debug.LogWarning("No Cards in the deck list!");
			return;
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
			cardScript.CardName.text = card.CardName;
			cardScript.CardText.text = card.CardText;
			cardScript.ManaCostText.text = card.ManaCost.ToString();
			// Attack/Health
			if(card.Attack != -1) {
				cardScript.AttackText.text = card.Attack.ToString();
			} else {
				cardScript.AttackText.text = string.Empty;
				cardScript.AttackIcon.sprite = null;
			}
			if(card.Health != -1) {
				cardScript.HealthText.text = card.Health.ToString();
			} else {
				cardScript.HealthText.text = string.Empty;
				cardScript.HealthIcon.sprite = null;
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
