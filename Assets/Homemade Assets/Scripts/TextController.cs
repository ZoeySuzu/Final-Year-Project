using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour {

    public Queue<string> TextList;
    public GameObject panel;
    public Text textToDisplay;
    private bool boxOpen;

	// Use this for initialization
	void Start () {
        textToDisplay.text = "";
        boxOpen = false;
        gameObject.SetActive(false);
    }

    void LateUpdate()
    {
        if (boxOpen && Input.GetButtonDown("Interact"))
        {
            displayNext();
        }
    }

    public void displayNext()
    {
        if (TextList.Count < 1)
        {
            boxOpen = false;
            gameObject.SetActive(false);
        }
        else
        {
            textToDisplay.text = TextList.Dequeue();
        }
    }

    public void setText(Queue<string> text)
    {
        gameObject.SetActive(true);
        boxOpen = true;
        TextList = text;
    }
    public bool getState()
    {
        return boxOpen;
    }
}
