using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Last clean: 31/01/2018

public class UIController : MonoBehaviour {
    public static UIController Instance { get; set; }

    //HUD text
    [SerializeField]
    private Text action;
    [SerializeField]
    private Text interaction;

    //Debuging display text
    [SerializeField]
    private Text state, spellType;

    //PauseScreen
    [SerializeField]
    private GameObject pauseScreen;

    private GameObject inventoryScreen;
    private GameObject relationshipScreen;

    [SerializeField]
    public GameObject MenuScreen;

    //TextDisplay
    private TextController textDisplay;
    private GameObject textBox;

    [SerializeField]
    private GameObject indicator;
    [SerializeField]
    private Text healthString, manaString;

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
    }

    // Use this for initialization
    void Start () {
        inventoryScreen = UI_Inventory.Instance.gameObject;
        inventoryScreen.SetActive(false);
        relationshipScreen = UI_Relationship.Instance.gameObject;
        relationshipScreen.SetActive(false);
        pauseScreen.SetActive(false);
    }
    void Update()
    {
        if (Input.GetButtonDown("B"))
        {
            //UIController.Instance.closeSubscreen();
        }
    }

    //get methods:
    public GameObject getInteractIcon()
    {
        return indicator;
    }

    public string getActionButton()
    {
        return action.text;
    }

    //Set methods:

    public void setActionButton(string actiontxt)
    {
        action.text = actiontxt;
    }

    public void setInteractButton(string interactiontxt)
    {
        interaction.text = interactiontxt;
    }

    public void setPlayerState(string statetxt)
    {
        state.text = "State: " + statetxt;
    }

    public void setSpellState(string statetxt)
    {
        spellType.text = "Spell: " + statetxt;
    }

    //Update methods

    public void updateHealth(Stat health)
    {
        healthString.text = health.statValue + "/" + health.statMax;
    }

    public void updateMana(Stat mana)
    {
        manaString.text = mana.statValue + "/" + mana.statMax;
    }

    //Various methods
    public void SaveGame()
    {
        GameController.Instance.SaveGame();
    }
    

    public void pause()
    {
        pauseScreen.SetActive(true);
    }
    public void resume()
    {
        inventoryScreen.SetActive(false);
        pauseScreen.SetActive(false);
    }

    public void openInventory()
    {
        pauseScreen.SetActive(false);
        inventoryScreen.SetActive(true);
    }

    public void openRelationships()
    {
        pauseScreen.SetActive(false);
        relationshipScreen.SetActive(true);
    }

    public void closeSubscreen()
    {
        pauseScreen.SetActive(true);
        inventoryScreen.SetActive(false);
        relationshipScreen.SetActive(false);
    }

}
