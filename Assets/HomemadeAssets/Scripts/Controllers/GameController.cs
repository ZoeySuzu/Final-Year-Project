using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Last clean: 29/11/2017

public class GameController : MonoBehaviour {
    public static GameController Instance { get; set; }

    [SerializeField]
    private Transform player;
    [SerializeField]
    private Transform ui;
    public LevelHandler levelSystem { get; private set; }

    //-----------------------------------Main game controllers:
    public UIController uic { get; private set; }
    public PlayerController pc { get; private set; }
    public SaveHandler saveSystem { get; private set; }

    public Inventory inventory;

    public List<GameObject> entities;
    public List<CharacterController> characters;
    public List<CameraID> cameraIds;

    [SerializeField]
    bool bypassMenu = false,disableUiControls = false;

    //-----------------------------------Attach game controllers on start:

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        Transform curUI = Instantiate(ui);
        curUI.SetParent(transform);
        curUI.name = "UI";
        uic = curUI.GetComponent<UIController>();
        
        inventory = new Inventory();
        inventory.money = new Collectible("Money", 100);
        inventory.addItem(new QuestItem("Academy Badge"));

        saveSystem = new SaveHandler();
        characters = new List<CharacterController>();
        cameraIds = new List<CameraID>();
        levelSystem = new LevelHandler();
        entities = new List<GameObject>();
    }

    private void debugStart()
    {
        if (bypassMenu)
        {
            spawnPlayer();
            uic.MenuScreen.SetActive(false);
        }
        if (disableUiControls)
        {
            uic.transform.FindChild("Debug").gameObject.SetActive(false);
        }
    }

    public void Start()
    {
        debugStart();
    }
    //-----------------------------------Quit to main menu:
    public void quit()
    {
        Debug.Log("Quit to main menu");
        FollowCamera.Instance.turnOff();
        Destroy(pc.gameObject);
        levelSystem.unloadAll();
        uic.MenuScreen.SetActive(true);
        Time.timeScale = 1;
    }


    //-----------------------------------Pause Game:
    public void pause()
    {
        pauseEntities();
        if (Time.timeScale == 1)
        {
            Debug.Log("pause");
            pc.enabled = false;
            Time.timeScale = 0;
        }
        else
        {
            Debug.Log("unpause");
            pc.enabled = true;
            Time.timeScale = 1;
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
        uic.MenuScreen.SetActive(false);
        levelSystem.initLevelList();
        levelSystem.loadLevel(levelSystem.levels[0]);
        spawnPlayer();
    }


    public void loadLevel()
    {
        uic.MenuScreen.SetActive(false);
        spawnPlayer();
        LoadGame();
        levelSystem.loadLevel(levelSystem.levels[0]);
        FollowCamera.Instance.turnOn();
       
    }

    public void quitGame()
    {
        Debug.Log("Quiting game via menu");
        Application.Quit();
        Debug.Break();
    }


    private void spawnPlayer()
    {
        Transform curplayer = Instantiate(player);
        curplayer.SetParent(transform.FindChildByRecursive("Entities"));
        curplayer.name = "Object_Player";
        pc = curplayer.GetComponent<PlayerController>();
        FollowCamera.Instance.turnOn();
    }
}