using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Last clean: 29/11/2017

public class UIController : MonoBehaviour {

    //HUD text
    public Text action;
    public Text interaction;

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

	// Use this for initialization
	void Start () {
        action.text = "";
        interaction.text = "";
        pauseScreen.SetActive(false);
        state.text = "";
        spellType.text = "";
    }
	
    //get methods:
    public GameObject getInteractIcon()
    {
        return indicator;
    }

    //Set methods:
    public void setInteractButton(string interactiontxt)
    {
        interaction.text = interactiontxt;
    }

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
