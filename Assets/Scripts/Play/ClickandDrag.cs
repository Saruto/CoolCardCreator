using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        IsDragging = !IsDragging;
    }
}
