using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickandDrag : MonoBehaviour
{
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
			IsDragging = false;
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			bool didHit = Physics.Raycast(ray, out hit);
			// Place card on valid spot
			if(didHit && hit.transform.GetComponent<Square>() != null && hit.transform.GetComponent<Square>().canPlace()) 
			{
				
				
			} 
			// Return card to hand.
			else 
			{
				LayoutRebuilder.MarkLayoutForRebuild(transform.parent.GetComponent<RectTransform>());
			}
		}
		else 
		{
			IsDragging = true;
			transform.rotation = Quaternion.identity;
		}		
    }
}
