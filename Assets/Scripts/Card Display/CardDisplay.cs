using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardDisplay : MonoBehaviour {

    CSVParser parser;

    // Used for displaying the deck
    [SerializeField] GameObject deckDisplay;

    [SerializeField] GameObject SearchBox;

    // Used for searching, and narrowing down cards
    List<Card> SearchedCards = new List<Card>();
    // Each card image
    GameObject[] CardImages = new GameObject[OFFSET_VALUE];

    // Used for tracking the offset of cards, for displaying the cards in the card display. Offset by values of 8
    int offset = 0;
    // Used for the offset value
    const int OFFSET_VALUE = 8;

	// Use this for initialization
	void Start () {
        // Find the parser to get access to the card list
        // Also eventual stop using .Find lol
        parser = GameObject.Find("Parser").GetComponent<CSVParser>();

        // Find each card image, and set them equal to the children of the card grid
        for (int i = 0; i < CardImages.Length; i++)
        {
            CardImages[i] = transform.GetChild(i).gameObject;
        }
        ResetCardList();
        // Display the card names
        UpdateCards();
	}
	
	// Update is called once per frame
	void Update () {
	}

    // Clear and repopulate the list
    void ResetCardList()
    {
        SearchedCards.Clear();
        for (int i = 0; i < parser.AllCards.Length; i++)
        {
            SearchedCards.Add(parser.AllCards[i]);
        }
    }

    void UpdateCards()
    {
        int length;

        // If the count is 
        if(SearchedCards.Count < offset + OFFSET_VALUE)
        {
            
            length = SearchedCards.Count - offset;
            for (int i = 0; i < OFFSET_VALUE - length; i++)
            {
                CardImages[OFFSET_VALUE - 1 - i].SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < CardImages.Length; i++)
            {
                CardImages[i].SetActive(true);
            }
            length = OFFSET_VALUE;
        }
        GenerateCardImages(length);
    }

    void GenerateCardImages(int length)
    {
        for (int i = 0; i < length; i++)
        {
            // Show the cards with their offset, incase we've pressed the arrows
			Utility.Instance.ApplyCardInfoToCardObject(SearchedCards[i + offset], CardImages[i].GetComponent<CardScript>());
        }
    }

    public void CardsForwards()
    {
        // Only add to the offset if it wont make it bigger than our card list
        if(offset + OFFSET_VALUE < SearchedCards.Count)
        {
            offset += OFFSET_VALUE;
        }
            
        UpdateCards();
    }

    public void CardsBackwards()
    {
        // Make sure we don't make the offset less than zero
        if(offset - OFFSET_VALUE < 0)
        {
            offset = 0;
        }
        else
        {
            offset -= OFFSET_VALUE;
        }
        UpdateCards();

    }

    public void AddToDeck()
    {
        int index = -1;
        for (int i = 0; i < CardImages.Length; i++)
        {
            if (EventSystem.current.currentSelectedGameObject == CardImages[i])
            {
                index = i;
                break;
            }
        }
        if(index != -1)
        {

            deckDisplay.GetComponent<DeckDisplay>().AddCard(SearchedCards[offset + index]);
        }
    }

    public void ValueChangeCheck()
    {
        if(SearchBox.GetComponent<InputField>().text == "")
        {
            ResetCardList();
            UpdateCards();
        }
        else
        {
            ResetCardList();
            RemoveCardsFromList(SearchBox.GetComponent<InputField>().text);
        }
    }

    void RemoveCardsFromList(string typedValue)
    {
        // Reset the offset when removing cards
        offset = 0;
        foreach(Card card in SearchedCards.ToArray())
        {
            string cardname = card.CardName.ToLower();
            if (!cardname.StartsWith(typedValue.ToLower()))
            {
                SearchedCards.Remove(card);
            }
        }
        UpdateCards();
    }
}
