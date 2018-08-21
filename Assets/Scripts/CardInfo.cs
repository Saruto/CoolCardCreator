using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
