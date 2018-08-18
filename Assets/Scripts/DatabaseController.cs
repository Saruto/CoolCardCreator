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
	// All of the cards parsed from the CSV
	List<Card> AllCards = new List<Card>();
	
	// --- Start --- //
	void Start () {

	}
	
	// --- Update --- //
	void Update () {
		
	}


	// --- Button Callbacks --- //
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

	public void OnMakeAllCards() {
		if(AllCards.Count == 0) {
			Debug.LogWarning("No Cards in internal cards list! Please click \"Parse CSV / Populate List\" first!");
			return;
		}

	}
}
