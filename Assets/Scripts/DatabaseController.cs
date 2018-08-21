using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// Represents a single row of card information from the CSV
public struct Card {
	public int ID;
	public string CardName;
	public string ManaCost;
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
	
	
	// The newline character this system uses.
	readonly static string NewlineCharacter = Environment.NewLine;


	// The canvas used for rendering the decks.
	[SerializeField] GameObject DeckRendererCanvas;

	// The camera that's currently rendering the deck.
	[SerializeField] Camera DeckRendererCamera;

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

	// The decklist text input field and the deck's name.
	[SerializeField] Text DeckName;
	[SerializeField] Text DeckList;

	// The deck color picker options and the current color selected.
	[SerializeField] Image[] DeckColorOptions;
	int CurrentlySelectedColor = -1;


	// Various Sprites
	[SerializeField] Sprite TransparentSprite;


	// The order of the columns in the CSV parser.
	enum CSVColumns { ID, CardName, ManaCost, LandType, Type, CardText, Attack, Health, Directions, Rarity };


	// ----------------------------------------- Methods ----------------------------------------- //
	// --- Start --- //
	void Start() {
		OnParseCSV();
		OnDeckColorPress(0);
	}

	// ----------- Button Callbacks ----------- //
	// Parses entire CSV and adds it to the AllCards list.
	public void OnParseCSV() {
		// Clear the list.
		AllCards.Clear();

		// Get CSV, strip the first row.
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
			card.ID = Convert.ToInt32(elements[(int)CSVColumns.ID]);
			card.CardName = elements[(int)CSVColumns.CardName];
			card.ManaCost = elements[(int)CSVColumns.ManaCost];
			card.Type = elements[(int)CSVColumns.Type];
			card.CardText = elements[(int)CSVColumns.CardText];
			if(elements[(int)CSVColumns.Attack] != "") {
				card.Attack = Convert.ToInt32(elements[(int)CSVColumns.Attack]);
			} else {
				card.Attack = -1;
			}
			if(elements[(int)CSVColumns.Health] != "") {
				card.Health = Convert.ToInt32(elements[(int)CSVColumns.Health]);
			} else {
				card.Health = -1;
			}
			Enum.TryParse(elements[(int)CSVColumns.Rarity], out card.Rarity);
			
			// Parse Directions
			string[] csvDirections = elements[(int)CSVColumns.Directions].Split('/');
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
			Card card = AllCards.Find((c) => c.CardName == numberCardNamePair[1]);
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
		MakeDeck(AllCards, "All Cards");
		print("Created all card gameobjects!");
	}

	
	// Saves the deck that is currently being rendered by the DeckRendererCamera.
	public void OnSaveDeck() {
		// Get name of deck, stripping out illegal characters.
		string deckName = null;
		foreach(Transform child in DeckRendererCanvas.transform) {
			string pattern = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
			Regex regex = new Regex(string.Format("[{0}]", Regex.Escape(pattern)));		
			deckName = regex.Replace(child.name, "");	 
		}
		if(deckName == null) {
			Debug.LogWarning("No deck in the renderer canvas! Please press one of the Deck Creation options first!");
			return;
		}
		string PATH = Application.persistentDataPath + "/" + deckName + " - " + DateTime.Now.ToString("MM-dd-yyyy-HH-mm-ss") + ".png";
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
