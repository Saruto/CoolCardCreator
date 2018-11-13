using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Test : MonoBehaviour {

    UnityWebRequest myWrGet;
    UnityWebRequest myWrPost;
    List<IMultipartFormSection> formSections;

    // Use this for initialization
    void Start () {
        myWrGet = UnityWebRequest.Get("https://card-game-server.herokuapp.com/");
        myWrPost = UnityWebRequest.Post("https://card-game-server.herokuapp.com/players", formSections);
        StartCoroutine(GetText());
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator GetText()
    {
        yield return myWrGet.SendWebRequest();

        if(myWrGet.isNetworkError || myWrGet.isHttpError)
        {
            print("Andrew sucks lmao");
        }
        else
        {
            Debug.Log(myWrGet.downloadHandler.text);
        }
    }
}
