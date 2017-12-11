﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Last clean: 29/11/2017

public class GameController : MonoBehaviour {
    public static GameController Instance { get; set; }

    //-----------------------------------Main game controllers:
    private UIController ui;
    private PlayerController pc;

    private ArrayList teleportPads;

    //-----------------------------------Attach game controllers on start:
    void Start () {
        ui = GetComponentInChildren<UIController>();
        pc = GetComponentInChildren<PlayerController>();
        
	}

    private void Awake()
    {
        teleportPads = new ArrayList();
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
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
        if (Time.timeScale == 1)
        {
            Debug.Log("pause");
            pc.enabled = false;
            Time.timeScale = 0;
            ui.pause();
        }
        else
        {
            Debug.Log("unpause");
            pc.enabled = true;
            Time.timeScale = 1;
            ui.resume();
        }
    }

    public void pauseEntities()
    {
        pc.enabled = !pc.enabled;
    }

    public void addTeleportPad(TeleportPad pad)
    {
        teleportPads.Add(pad);
    }

    public ArrayList getTeleportPads()
    {
        return teleportPads;
    }

}
