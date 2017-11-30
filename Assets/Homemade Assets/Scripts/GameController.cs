using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Last clean: 29/11/2017

public class GameController : MonoBehaviour {

    //-----------------------------------Main game controllers:
    private UIController ui;
    private PlayerController pc;

    //-----------------------------------Attach game controllers on start:
    void Start () {
        ui = GetComponentInChildren<UIController>();
        pc = GetComponentInChildren<PlayerController>();
	}


    //-----------------------------------Listen for pause:
    void Update () {
        if (Input.GetButtonDown("Start"))
        {
            pause(); 
        }
	}


    //-----------------------------------Quit to main menu:
    public void quit()
    {
        Debug.Log("Quit to main menu");
        SceneManager.LoadSceneAsync("MainMenu",LoadSceneMode.Single);
        pause();
    }


    //-----------------------------------Pause Game:
    public void pause()
    {
        print("Start pressed");
        if (Time.timeScale == 1)
        {
            Debug.Log("pasue");
            pc.enabled = false;
            Time.timeScale = 0;
            showPaused();
        }
        else
        {

            Debug.Log("unpause");
            pc.enabled = true;
            Time.timeScale = 1;
            hidePaused();
        }
    } 

    private void showPaused()
    {
        ui.pause();
    }

    private void hidePaused()
    {
        ui.resume();
    }

}
