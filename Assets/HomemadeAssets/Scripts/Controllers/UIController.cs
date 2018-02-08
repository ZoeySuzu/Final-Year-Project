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
        pauseScreen.SetActive(false);
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

    public void updateHealth(int health)
    {
        healthString.text = health + "/" + PlayerController.Instance.getMaxHP();
    }

    public void updateMana(int mana)
    {
        manaString.text = mana + "/" + PlayerController.Instance.getMaxMana();
    }

    //Various methods
    public void pause()
    {
        pauseScreen.SetActive(true);
        pauseScreen.transform.GetChild(1).GetComponent<Button>().Select();
    }
    public void resume()
    {
        pauseScreen.SetActive(false);
    }
}
