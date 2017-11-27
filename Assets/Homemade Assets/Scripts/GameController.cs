using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    UIController ui;
    public GameObject player;
    private PlayerController pc;

	// Use this for initialization
	void Start () {
        ui = GetComponentInChildren<UIController>();
        pc = GetComponentInChildren<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Start"))
        {
            pause(); 
        }
	}

    public void quit()
    {
        Debug.Log("Quit to main menu");
        SceneManager.LoadSceneAsync("MainMenu",LoadSceneMode.Single);
    }
    public void pause()
    {
        print("Start pressed");
        if (Time.timeScale == 1)
        {
            Debug.Log("pasue");
            pc.playerInactive();
            Time.timeScale = 0;
            showPaused();
        }
        else
        {

            Debug.Log("unpause");
            pc.playerActive();
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
