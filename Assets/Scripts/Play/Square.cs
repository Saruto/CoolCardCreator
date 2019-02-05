using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{


	public bool isActive { get; private set; } = true;
 	
	// Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	// Returns true if the given card can be placed on this square. False otherwise.
	public bool canPlace() {
		return isActive;
	}
}
