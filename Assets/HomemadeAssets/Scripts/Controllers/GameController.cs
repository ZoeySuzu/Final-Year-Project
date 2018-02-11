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
    private SaveHandler saveSystem;

    public List<GameObject> entities;
    public List<CharacterController> characters;
    public List<TeleportPad> teleportPads;
    public List<CameraID> cameraIds;

    [SerializeField]
    GameObject camera;

    //-----------------------------------Attach game controllers on start:

    private void Awake()
    {
        saveSystem = new SaveHandler();
        characters = new List<CharacterController>();
        cameraIds = new List<CameraID>();
        teleportPads = new List<TeleportPad>();
        entities = new List<GameObject>();
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void Start()
    {
        ui = UIController.Instance;
        pc = PlayerController.Instance;
        pc.gameObject.SetActive(false);
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


    public void addTeleportPad(TeleportPad pad)
    {
        Debug.Log("Added to teleport list " + pad.getName());
        teleportPads.Add(pad);
    }

    public List<TeleportPad> getTeleportPads()
    {
        return teleportPads;
    }

    public void loadLevel(string levelName, Transform teleport)
    {
        SceneManager.LoadScene(levelName, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(1);
        pc.transform.position = teleport.position;
    }

    public void SaveGame()
    {
        saveSystem.SaveGame();
    }

    public void LoadGame()
    {
        saveSystem.LoadGame();
    }

    public void newGame()
    {
        camera.SetActive(false);
        ui.MenuScreen.SetActive(false);
        SceneManager.LoadScene("DebugLevel", LoadSceneMode.Additive);
        pc.gameObject.SetActive(true);
    }

    public void loadLevel()
    {
        camera.SetActive(false);
        ui.MenuScreen.SetActive(false);
        SceneManager.LoadScene("DebugLevel", LoadSceneMode.Additive);
        pc.gameObject.SetActive(true);
        LoadGame();
        
    }

    public void quitGame()
    {
        Debug.Log("Quiting game via menu");
        Application.Quit();
        Debug.Break();
    }

}
