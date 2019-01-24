using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class CardDisplay : MonoBehaviour {

    CSVParser parser;

    // Used for displaying the deck
    [SerializeField] GameObject deckDisplay = null;

    [SerializeField] GameObject SearchBox = null;

    // Used for searching, and narrowing down cards
    List<Card> SearchedCards = new List<Card>();
    // Each card image
    GameObject[] CardImages = new GameObject[OFFSET_VALUE];

    // Used for tracking the offset of cards, for displaying the cards in the card display. Offset by values of 8
    int offset = 0;
    // This is equal to the number of card images that appear on the screen, 8 by default, but can be changed later
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

    // Clear and repopulate the list this happens whenever a search box is cleared,
    // or a manacost button is unchecked
    void ResetCardList()
    {
        SearchedCards.Clear();
        for (int i = 0; i < parser.AllCards.Length; i++)
        {
            SearchedCards.Add(parser.AllCards[i]);
        }
        UpdateCards();
    }

    // This updates the visuals of the cards.
    // This function is important when you press the left or right buttons,
    // and when you do anything that narrows the list of cards
    void UpdateCards()
    {
        int length;

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

    // This grabs how the cards should look (mana cost, name etc) and
    // generates that on the cards, including their offset
    // The length variable is primarily for when a page has less cards than the expected offset
    // i.e. if the offset is 8, we need to disable some cards when there's only 7 to display
    void GenerateCardImages(int length)
    {
        for (int i = 0; i < length; i++)
        {
            // Show the cards with their offset, incase we've pressed the arrows
			Utility.Instance.ApplyCardInfoToCardObject(SearchedCards[i + offset], CardImages[i].GetComponent<CardScript>());
        }
    }

    // This function moves all the cards forwards, obviously moving the offset forwards as well.
    // This will not move once there are no more cards to offset
    public void CardsForwards()
    {
        // Only add to the offset if it wont make it bigger than our card list
        if(offset + OFFSET_VALUE < SearchedCards.Count)
        {
            offset += OFFSET_VALUE;
        }
            
        UpdateCards();
    }

    // This function moves the cards back, as well as changing the offset back.
    // This function will not go below zero
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

    // Handles adding cards to your deck, it passes a card into the deck display, which
    // is a list that stores the cards actually in the deck
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

    // Function that handles the search bar
    public void ValueChangeCheck()
    {
        ResetCardList();
        // When the search bar is empty, we need to reset the card list completely and 
        if (SearchBox.GetComponent<InputField>().text == "")
        {
            UpdateCards();
        }
        else
        {
            RemoveBySearch(SearchBox.GetComponent<InputField>().text);
        }
    }

    // Create a list of the cards that start with a certain string,
    // then remove those cards from the searched cards list
    void RemoveBySearch(string typedValue)
    {
        List<Card> removeCards = new List<Card>();
        for (int i = 0; i < parser.AllCards.Length; i++)
        {
            // Loop through all the cards, if the cards start with that string
            // then add them to the remove cards list
            string cardname = parser.AllCards[i].CardName.ToLower();
            if (!cardname.StartsWith(typedValue.ToLower()))
            {
                removeCards.Add(parser.AllCards[i]);
            }
        }
        // Call the remove cards function
        RemoveCardsFromList(removeCards);
    }

    // Remove cards from the searched cards list so that we
    // can refine our searches
    void RemoveCardsFromList(List<Card> cardsToRemove)
    {
        // Reset the offset when removing cards
        offset = 0;
        for (int i = 0; i < cardsToRemove.Count; i++)
        {
            // Remove all the cards in the cards to remove list
            SearchedCards.Remove(cardsToRemove[i]);
        }
        UpdateCards();
    }

    
}
