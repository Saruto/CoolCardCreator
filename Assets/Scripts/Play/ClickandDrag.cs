using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickandDrag : MonoBehaviour
{
	public GameObject CardObjectPrefab;

	bool IsDragging;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(IsDragging)
        {
            transform.position = Input.mousePosition;
        }
    }

    // Function that the button calls which allows us to drag the UI object around
    public void Drag()
    {
		if(IsDragging) 
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			bool didHit = Physics.Raycast(ray, out hit);
			Square squareScript = didHit ? hit.transform.GetComponent<Square>() : null;
			// Place card on valid spot
			if(didHit && squareScript != null && squareScript.canPlace()) 
			{
				//GameObject cardObj = Instantiate(CardObjectPrefab);
				GameObject cardObj = Instantiate(Resources.Load("Cards/" + GetComponent<CardScript>().card.CardName) as GameObject);
				cardObj.transform.SetParent(squareScript.transform);
				cardObj.transform.localPosition = new Vector3(0, 0.55f, 0);
				cardObj.GetComponentInChildren<CardScript>().UpdateCardVisuals(transform.GetComponent<CardScript>().card);

				cardObj.GetComponent<UnitCard>().OnPlayed();

				Destroy(gameObject);
			} 
			// Remake the hand and make it no longer dragging.
			GetComponent<LayoutElement>().ignoreLayout = false;
			IsDragging = false;
			PlayerInput.Instance.isDraggingCard = false;
		}
		else if(!PlayerInput.Instance.isDraggingCard)
		{
			IsDragging = true;
			PlayerInput.Instance.isDraggingCard = true;
			transform.rotation = Quaternion.identity;
			GetComponent<LayoutElement>().ignoreLayout = true;
		}		
    }
}
