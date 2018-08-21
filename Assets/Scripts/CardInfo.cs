using System;
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


// Script attached to all card gameobjects that define all of its info fields.
public class CardInfo : MonoBehaviour {
	// References to the various GOs on the template.
	public Image Background;
	public Image UpArrow;
	public Image DownArrow;
	public Image LeftArrow;
	public Image RightArrow;
	public Image AttackIcon;
	public Image HealthIcon;

	public Text CardName;
	public Text CardText;
	public Text AttackText;
	public Text HealthText;

	public GameObject ManaCostLayout;
}
