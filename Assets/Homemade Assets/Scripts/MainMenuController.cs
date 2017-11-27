using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    public void newGame()
    {
        SceneManager.LoadSceneAsync("TestDev", LoadSceneMode.Single);

    }

    public void loadLevel()
    {

    }

    public void quitGame()
    {
        Debug.Log("Quiting game via menu");
        Application.Quit();
        Debug.Break();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
