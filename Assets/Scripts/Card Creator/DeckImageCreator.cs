using System;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine;

// Handles the creation and saving of deck images.
public class DeckImageCreator : MonoBehaviour {
	// The canvas used for rendering the decks.
	[SerializeField] GameObject DeckRendererCanvas = null;

	// The camera that's currently rendering the deck.
	[SerializeField] Camera DeckRendererCamera = null;

	// Should we save to the Project folder path? Or to the AppData folder?
	enum SaveLocation { ProjectFolder, AppDataFolder }
	[SerializeField] SaveLocation SaveDeckLocation = SaveLocation.ProjectFolder;

	// The root folder containing all decks.
	static string DIRECTORY_PATH;


	// --- Awake --- //
	void Awake() {
		if(SaveDeckLocation == SaveLocation.ProjectFolder) {
			DIRECTORY_PATH = Application.dataPath + "/Created Decks/";
		} else {
			DIRECTORY_PATH = Application.persistentDataPath + "/Created Decks/";
		}
	}


	// ----------- Button Callbacks ----------- //
	// Saves the deck that is currently being rendered by the DeckRendererCamera.
	public void OnSaveDeck() {
		// Get name of deck, stripping out illegal characters.
		string deckName = null;
		foreach(Transform child in DeckRendererCanvas.transform) {
			string pattern = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
			Regex regex = new Regex(string.Format("[{0}]", Regex.Escape(pattern)));		
			deckName = regex.Replace(child.name, "");	 
		}
		if(deckName == null) {
			Debug.LogWarning("No deck in the renderer canvas! Please press one of the Deck Creation options first!");
			return;
		}
		string PATH = DIRECTORY_PATH + deckName + " - " + DateTime.Now.ToString("MM-dd-yyyy-HH-mm-ss") + ".png";
		// Create texture from the DeckRendererCamera's target texture.
		Texture2D texture = new Texture2D(4000, 2800, TextureFormat.ARGB32, false);
		RenderTexture.active = DeckRendererCamera.targetTexture;
		texture.ReadPixels(new Rect(0, 0, 4000, 2800), 0, 0);
		texture.Apply();
		RenderTexture.active = null;
		// Encode texture to png and save it.
		byte[] bytes = texture.EncodeToPNG();
		System.IO.File.WriteAllBytes(PATH, bytes);
		print("Deck image creation successful! Saved to: " + PATH);
	}
}
