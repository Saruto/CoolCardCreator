using System;
using System.Linq;
using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine;
using FileHelpers;

// Handles reading data from the CSV and turning it into Card Structs.
public class CSVParser : MonoBehaviour {
	// All of the cards parsed from the CSV
	public Card[] AllCards { get; private set; }

	// --- Start --- //
	void Awake() {
		OnParseCSV();
	}

	// ----------- Button Callbacks ----------- //
	// Parses entire CSV, turning data from the CSV into Card struct objects.
	// From Card Entry in CSV -> Card Struct
	public void OnParseCSV() {
		FileHelperEngine<Card> engine = new FileHelperEngine<Card>();
		engine.BeforeReadRecord += (eng, e) => {
			// Ignore lines with names starting with "[", or lines with empty names.
			if(e.RecordLine.StartsWith("[") || e.RecordLine.StartsWith(",")) {
				e.SkipThisRecord = true;
			}
		};
		AllCards = engine.ReadFile(Application.streamingAssetsPath + "/Card Database.csv");
	}
}
