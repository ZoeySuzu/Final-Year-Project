using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Last clean: 29/11/2017

public class MainMenuController : MonoBehaviour {


    //------------------------------------------Menu Methods

    public void newGame()
    {
        SceneManager.LoadSceneAsync("TestDev", LoadSceneMode.Single);
        SceneManager.LoadSceneAsync("DebugLevel", LoadSceneMode.Additive);
    }

    public void loadLevel()
    {
        //Not implemented yet
    }

    public void quitGame()
    {
        Debug.Log("Quiting game via menu");
        Application.Quit();
        Debug.Break();
    }
}
