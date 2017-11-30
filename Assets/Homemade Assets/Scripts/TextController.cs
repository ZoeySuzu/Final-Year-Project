using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour {
    public static TextController Instance { get; set; }

    public Queue<string> TextList;
    public GameObject textPanel;
    public QuestionController question;
    public Text textToDisplay;
    private bool newDialogue;
    private bool dialogueLeft;
    private bool askingQuestion;

    private void Awake()
    {
        if(Instance != null && Instance != this)
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
        textToDisplay.text = "";
        newDialogue = false;
        gameObject.SetActive(false);
    }

    public void setText(Queue<string> text, string name)
    { 
        TextList = text;
    }

    public void displayText()
    {
        newDialogue = true;
        dialogueLeft = true;
        gameObject.SetActive(true);
        displayNext();
    }

    void Update()
    {
        if (newDialogue)
        {
            newDialogue = false;
        }
        else if (askingQuestion)
        {

        }
        else if (dialogueLeft && Input.GetButtonDown("Interact"))
        {
            displayNext();
        }
    }

    public void displayNext()
    {
        if (TextList.Count < 1)
        {
            dialogueLeft = false;
            gameObject.SetActive(false);
        }
        else
        {
            string text = TextList.Dequeue();
            if (text == "Question")
            {
                askingQuestion = true;
                textToDisplay.text = TextList.Dequeue();
                question.setAwnsers(TextList.Dequeue(), TextList.Dequeue());
            }
            else
            {
                textToDisplay.text = text;
            }
        }
    }

    public void answer(int answer)
    {
        askingQuestion = false;
    }

    public bool getState()
    {
        return gameObject.activeSelf;
    }
}
