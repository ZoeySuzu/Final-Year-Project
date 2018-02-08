using System.Collections;
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
    private ArrayList entities;
    private ArrayList cameraIds;

    //-----------------------------------Attach game controllers on start:
    void Start () {
        ui = GetComponentInChildren<UIController>();
        pc = GetComponentInChildren<PlayerController>();        
	}

    private void Awake()
    {
        cameraIds = new ArrayList();
        teleportPads = new ArrayList();
        entities = new ArrayList();
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
        FollowCamera.Instance.enabled = FollowCamera.Instance.enabled;
        pauseEntities();
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
        foreach (GameObject go in entities)
        {

        }
    }
    public void pauseEntities(bool state)
    {
        pc.enabled = !state;
        foreach (GameObject go in entities) {
            
        }
    }

    public void addCameraId(CameraID c)
    {
        cameraIds.Add(c);
    }

    public ArrayList getCameraIds()
    {
        return cameraIds;
    }

    public void addEntity(GameObject go)
    {
        entities.Add(go);
    }

    public ArrayList getEntities()
    {
        return entities;
    }


    public void addTeleportPad(TeleportPad pad)
    {
        Debug.Log("Added to teleport list " + pad.getName());
        teleportPads.Add(pad);
    }

    public ArrayList getTeleportPads()
    {
        return teleportPads;
    }

    public void loadLevel(string levelName, Transform teleport)
    {
        SceneManager.LoadScene(levelName, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(1);
        pc.transform.position = teleport.position;
    }


}
