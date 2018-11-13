using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardDisplay : MonoBehaviour {

    CSVParser parser;

    // Used for displaying the deck
    [SerializeField] GameObject deckDisplay;
    // Each card image
    GameObject[] CardImages = new GameObject[8];

    // Used for tracking the offset of cards, for displaying the cards in the card display. Offset by values of 8
    int offset = 0;

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
        // Display the card names
        UpdateCards();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void UpdateCards()
    {
        int length;

        // If this is true, we know we're at the last cards, so we shouldn't update anything when we move the offset
        if(parser.AllCards.Length == offset)
        {
            length = 0;
        }
        // If the offset would be larger than the index of the cards, make it go to the max index
        else if(parser.AllCards.Length < offset + 8)
        {
            length = ((8 + offset) - parser.AllCards.Length);
        }
        // Otherwise it's just equal to the lenght of card images (8)
        else
        {
            length = CardImages.Length;
        }
        GenerateCardImages(length);
    }

    void GenerateCardImages(int length)
    {
        for (int i = 0; i < length; i++)
        {
            // Show the cards with their offset, incase we've pressed the arrows
            CardImages[i].transform.GetChild(0).GetComponent<Text>().text = parser.AllCards[i + offset].CardName;

        }
    }

    public void CardsForwards()
    {
        // Only add to the offset if it wont make it bigger than our card list
        if(offset + 8 < parser.AllCards.Length)
        {
            offset += 8;
        }
            
        UpdateCards();
    }

    public void CardsBackwards()
    {
        // Make sure we don't make the offset less than zero
        if(offset - 8 < 0)
        {
            offset = 0;
        }
        else
        {
            offset -= 8;
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

            deckDisplay.GetComponent<DeckDisplay>().AddCard(parser.AllCards[offset + index]);
        }
    }
}
