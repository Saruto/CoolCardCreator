using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FileHelpers;

// Enum flag, used to keep track of attack directions.
// For example: "Directions dir = Directions.Up | Directions.Left;" represents a unit with the up and left attack directions.
// dir.HasFlag(Directions.Up) function can be used to check if an enum has a flag set. 
[Flags]
public enum Directions { None = 0, Up = 1, Down = 2, Left = 4, Right = 8 }

// Used to keep track of Rarity.
public enum Rarity { Common, Uncommon, Rare, Mythic }

// Represents a single row of card information from the CSV
[DelimitedRecord(",")]
[IgnoreFirst(1)]
[IgnoreEmptyLines()]
public class Card {
	[FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForRead)]
	public string CardName;

	public string ManaCost;

	[FieldNullValue(typeof(char), " ")]
	public char LandType;

	public string Type;

	[FieldNullValue(typeof(string), "")]
	[FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForRead)]
	public string CardText;

	[FieldNullValue(typeof(int), "-1")]
	public int Attack;

	[FieldNullValue(typeof(int), "-1")]
	public int Health;

	[FieldConverter(typeof(DirectionsConverter))]
	[FieldNullValue(typeof(Directions), "None")]
	public Directions Directions;

	[FieldNullValue(typeof(Rarity), "Common")]
	public Rarity Rarity;

	public override string ToString() {
		return CardName;
	}
}

// Converter used for parsing the direction string to an enum.
public class DirectionsConverter : ConverterBase {
	public override object StringToField(string from) {
		string[] csvDirections = from.Split('/');
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
		return directions;
	}
	public override string FieldToString(object from) {
		return from.ToString();
	}
}
