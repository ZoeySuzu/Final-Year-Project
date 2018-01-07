using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Last clean: 29/11/2017

public class UIController : MonoBehaviour {
    public static UIController Instance { get; set; }
    //HUD text
    public Text action;

    //Debuging display text
    public Text state;
    public Text spellType;

    //PauseScreen
    public GameObject pauseScreen;

    //TextDisplay
    private TextController textDisplay;
    public GameObject textBox;

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
        action.text = "";
        pauseScreen.SetActive(false);
        state.text = "";
        spellType.text = "";
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

    public void setPlayerState(string statetxt)
    {
        state.text = "State: " + statetxt;
    }

    public void setSpellState(string statetxt)
    {
        spellType.text = "Spell: " + statetxt;
    }

    public void updateHealth(int health)
    {
        healthString.text = health + "/" + PlayerController.Instance.getMaxHP();
    }

    public void updateMana(int mana)
    {
        manaString.text = mana + "/" + PlayerController.Instance.getMaxMana();
    }


    public void pause()
    {
        pauseScreen.SetActive(true);
        pauseScreen.transform.GetChild(2).GetComponent<Button>().Select();
        pauseScreen.transform.GetChild(1).GetComponent<Button>().Select();
    }
    public void resume()
    {
        pauseScreen.SetActive(false);
    }
}
