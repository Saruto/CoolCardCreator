using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class DeckDisplay : MonoBehaviour {

    // Make a list that stores the deck
    List<Card> deckList = new List<Card>();
    [SerializeField] GameObject cardDeckImage;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void UpdateDeckVisuals()
    {
        // Sort by mana cost, then by name
        deckList = deckList.OrderBy(c => deckManaCostToInt(c)).ThenBy(c => c.CardName).ToList();

        List<Tuple<Card, int>> CompactedDeckList = new List<Tuple<Card, int>>();

        for (int i = 0; i < deckList.Count; i++)
        {
            // If the last item in the compacted list is the same as the current item in the for loop
            // We want to add one to the integer in the tuple
            if(i != 0 && CompactedDeckList[CompactedDeckList.Count - 1].Item1.CardName == deckList[i].CardName)
            {
                // Store the old value of the int in the tuple list
                int oldValue = CompactedDeckList[CompactedDeckList.Count - 1].Item2;
                // Delete the current tuple at the last index in the list
                CompactedDeckList.RemoveAt(CompactedDeckList.Count - 1);
                // Add the new tuple with a properly incremented value
                CompactedDeckList.Add(new Tuple<Card, int>(deckList[i], ++oldValue));
            }
            // If the last card in the compacted list is different from the item currently being added, just add
            // the new item, with an amount equal to one
            else
            {
                CompactedDeckList.Add(new Tuple<Card, int>(deckList[i], 1));
            }
        }

        // Create/Destroy the number of cards necessary based on the new cards added or removed
        if(transform.childCount < CompactedDeckList.Count)
        {
            int length = CompactedDeckList.Count - transform.childCount;
            // Loop creating new deck card prefabs until the number of those we have equals our decklist count
            for (int i = 0; i < length; i++)
            {
                GameObject card = Instantiate(cardDeckImage, transform);
            }
        }
        else if(transform.childCount > CompactedDeckList.Count)
        {
            int length = transform.childCount - CompactedDeckList.Count;
            // Loop creating new deck card prefabs until the number of those we have equals our decklist count
            for (int i = 0; i < length; i++)
            {
                Destroy(transform.GetChild(i));
            }
        }

        // Create, and rename prefabs based on the new card sort
        for (int i = 0; i < CompactedDeckList.Count; i++)
        {
            transform.GetChild(i).GetChild(0).GetComponent<Text>().text = CompactedDeckList[i].Item1.CardName;
            transform.GetChild(i).GetChild(1).GetComponent<Text>().text = CompactedDeckList[i].Item2.ToString();
        }
    }

    int deckManaCostToInt(Card card)
    {
        int cost = 0;
        IEnumerable<char> manaSymbols = card.ManaCost.ToCharArray();
        foreach (char symbol in manaSymbols)
        {
            switch (symbol)
            {
                case 'P': case 'R': case 'A': case 'G': case 'Y': cost++; break;
                default:
                    cost += (int)char.GetNumericValue(symbol);
                    break;
            }
        }
        return cost;
    }

    public void AddCard(Card newCard)
    {
        // Add a card to the decklist, and update a visual for it, as well as it's name
        deckList.Add(newCard);
        UpdateDeckVisuals();

    }

    public void RemoveCard()
    {

    }
}
