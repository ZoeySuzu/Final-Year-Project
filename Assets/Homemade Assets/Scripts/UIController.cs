using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    public Text action;
    public Text interaction;
    public Text pausetext;
    public Text state;
    private TextController textDisplay;
    public GameObject textBox;

	// Use this for initialization
	void Start () {
        action.text = "";
        interaction.text = "";
        pausetext.enabled = false;
        state.text = "";
    }
	

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

    public void pause()
    {
        pausetext.enabled = true;
    }
    public void resume()
    {
        pausetext.enabled = false;
    }

    public void displayText(Queue<string> text)
    {
        if (textBox.activeSelf == false){
            textBox.SetActive(true);
            textDisplay.setText(text);
        }
    }
}
