using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Test : MonoBehaviour {
	// ----------------------------------------- Internal Classes ----------------------------------------- //	
	// Player class
	class Player {
		public Player(string name_) { name = name_; }
		public string name;
	}

	// Request Class
	class PlayerInitResponse {
		public int id;
		public string secret;
		public string name;
	}

	// ----------------------------------------- Fields and Properties ----------------------------------------- //	

	// Secret key
	string secretKey;



	// ----------------------------------------- Methods ----------------------------------------- //
    // Use this for initialization
    void Start () {
		Player p1 = new Player("test name");
		StartCoroutine(PostPlayerInfo(p1));
    }
	
	// Sends a request to the server to create a new player. Stores the secret key returned from it.
	IEnumerator PostPlayerInfo(Player player) 
	{
		// Create Json Data to send
		string jsonData = JsonUtility.ToJson(player);	// If player.name == "Shafournee", then jsonData = "{\"name\", \"Shafournee\"}"

		// The URL to post the player information to
		const string URL = "https://card-game-server.herokuapp.com/players";

		// Create the web request. Initially set it as a Put request.
		using(UnityWebRequest request = UnityWebRequest.Put(URL, jsonData)) 
		{
			// Change the request to a POST request instead
			request.method = UnityWebRequest.kHttpVerbPOST;
			// Change the request to say we're sending a json object.
			request.SetRequestHeader("Content-Type", "application/json");
			request.SetRequestHeader("Accept", "application/json");
			
			// Send the request, and wait until we get it back.
			yield return request.SendWebRequest();

			// Check the request to make sure it's good
			if(!request.isNetworkError && request.responseCode < 400) 
			{
				Debug.Log("Data Sent to server. Return value: " + request.downloadHandler.text);
				PlayerInitResponse response = JsonUtility.FromJson<PlayerInitResponse>(request.downloadHandler.text);
				secretKey = response.secret;
			}
			else 
			{
				Debug.Log("Error sending or recieving response");
			}
		}
	}


	// Button Callback for the buttons on the Tic Tac Toe board
	public void OnCellChosen(int chosenCell) {
		Debug.Log("Button " + chosenCell + " clicked :3");
		// If it's your turn

		// Make a POST request to server with "clicked" message.

	}



}
