using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// Contains a bunch of useful functions 
public class Utility : MonoBehaviour {

	// Seralized Fields
	// Prefab for a mana symbol
	[SerializeField] GameObject ManaSymbolPrefab = null;

	// Prefabs for every mana symbol sprite
	[SerializeField] Sprite PurpleSymbol = null;
	[SerializeField] Sprite RedSymbol = null;
	[SerializeField] Sprite GraySymbol = null;
	[SerializeField] Sprite GreenSymbol = null;
	[SerializeField] Sprite YellowSymbol = null;
	[SerializeField] Sprite NeutralSymbol = null;
	// Other Sprites
	[SerializeField] Sprite AttackIconSprite = null;
	[SerializeField] Sprite HealthIconSprite = null;
	[SerializeField] Sprite TransparentSprite = null;


	// Singleton instance
	public static Utility Instance = null;
	


	// --- Awake --- //
	void Awake () {
		// Make this a singleton
		if(Instance == null) {
			Instance = this;
			DontDestroyOnLoad(gameObject);
		} else {
			Destroy(gameObject);
		}
	}

	// ------ Utility Functions ------ //
	// Takes in a Card and a card gameobject, and applies the card's information to the card object.
	public void ApplyCardInfoToCardObject(Card card, CardScript cardObject) {
		// Set fields
		//cardObject.Background.color = DeckColorOptions[CurrentlySelectedColor].color;
		cardObject.CardName.text = card.CardName;
		cardObject.CardText.text = card.CardText;
		// Mana
		foreach(Transform child in cardObject.ManaCostLayout.transform) {
			Destroy(child.gameObject);
		}
		IEnumerable<char> manaSymbols = card.ManaCost.ToCharArray();
		foreach(char symbol in manaSymbols) {
			GameObject symbolIcon = Instantiate(ManaSymbolPrefab, cardObject.ManaCostLayout.transform);
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
		// Land
		foreach(Transform child in cardObject.LandLayout.transform) {
			Destroy(child.gameObject);
		}
		if(card.LandType != ' ') {
			GameObject symbolIcon = Instantiate(ManaSymbolPrefab, cardObject.LandLayout.transform);
			switch(card.LandType) {
			case 'P': symbolIcon.GetComponent<Image>().sprite = PurpleSymbol; break;
			case 'R': symbolIcon.GetComponent<Image>().sprite = RedSymbol; break;
			case 'A': symbolIcon.GetComponent<Image>().sprite = GraySymbol; break;
			case 'G': symbolIcon.GetComponent<Image>().sprite = GreenSymbol; break;
			case 'Y': symbolIcon.GetComponent<Image>().sprite = YellowSymbol; break;
			}
		}
		// Attack/Health
		if(card.Attack != -1) {
			cardObject.AttackText.text = card.Attack.ToString();
			cardObject.AttackIcon.sprite = AttackIconSprite;
		} else {
			cardObject.AttackText.text = string.Empty;
			cardObject.AttackIcon.sprite = TransparentSprite;
		}
		if(card.Health != -1) {
			cardObject.HealthText.text = card.Health.ToString();
			cardObject.HealthIcon.sprite = HealthIconSprite;
		} else {
			cardObject.HealthText.text = string.Empty;
			cardObject.HealthIcon.sprite = TransparentSprite;
		}
		// Directions
		cardObject.UpArrow.enabled = false;
		cardObject.DownArrow.enabled = false;
		cardObject.LeftArrow.enabled = false;
		cardObject.RightArrow.enabled = false;
		if(card.Directions.HasFlag(Directions.Up)) {
			cardObject.UpArrow.enabled = true;
		}
		if(card.Directions.HasFlag(Directions.Down)) {
			cardObject.DownArrow.enabled = true;
		}
		if(card.Directions.HasFlag(Directions.Left)) {
			cardObject.LeftArrow.enabled = true;
		}
		if(card.Directions.HasFlag(Directions.Right)) {
			cardObject.RightArrow.enabled = true;
		}
	}
	
}
