using System;
using System.Linq;
using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine;

// Handles reading data from the CSV and turning it into Card Structs.
public class CSVParser : MonoBehaviour {
	// All of the cards parsed from the CSV
	public List<Card> AllCards { get; private set; } = new List<Card>();

	// The newline character this system uses.
	readonly static string NewlineCharacter = Environment.NewLine;

	// The order of the columns in the CSV parser.
	enum CSVColumns { CardName, ManaCost, LandType, Type, CardText, Attack, Health, Directions, Rarity };



	// --- Start --- //
	void Awake() {
		OnParseCSV();
	}

	// ----------- Button Callbacks ----------- //
	// Parses entire CSV, turning data from the CSV into Card struct objects.
	// From Card Entry in CSV -> Card Struct
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
		int id = 0;
		foreach(string row in rows) {
			// Splits by commas, unless an element is surrounded with double quotes.
			Regex rowSplitter = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
			string[] elements = rowSplitter.Split(row);

			// Need to do this to strip out random newline characters and the double quotes.
			elements = elements.Select((element) => { return Regex.Replace(element, @"\t|\n|\r", ""); }).ToArray();
			elements = elements.Select((element) => { return Regex.Replace(element, "\"", ""); }).ToArray();

			// Ignore rows with empty names or rows with names that begin with a '[' character.
			if(elements[0] == "" || elements[0].ToCharArray()[0] == '[') {
				continue;  
			}

			// Assign variables.
			Card card = new Card();
			card.ID = id++;
			card.CardName = elements[(int)CSVColumns.CardName];
			card.ManaCost = elements[(int)CSVColumns.ManaCost];
			card.Type = elements[(int)CSVColumns.Type];
			card.CardText = elements[(int)CSVColumns.CardText];
			if(elements[(int)CSVColumns.LandType] != "") {
				card.LandType = elements[(int)CSVColumns.LandType][0];
			} else {
				card.LandType = ' ';
			}
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
}
