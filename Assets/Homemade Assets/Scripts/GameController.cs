using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    UIController ui;
    public GameObject player;
    public bool fighting;

	// Use this for initialization
	void Start () {
        ui = GetComponentInChildren<UIController>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Start"))
        {
            print("Start");
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
                showPaused();
            }
            else if (Time.timeScale == 0)
            {
                Debug.Log("high");
                Time.timeScale = 1;
                hidePaused();
            }
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
