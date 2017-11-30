using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour {
    public static TextController Instance { get; set; }

    [SerializeField]
    private GameObject questionPanel;

    private Text textToDisplay;
    private Queue<string> textList;
    private bool newDialogue;
    private bool askingQuestion;

    private string[] pointer;
    private string talkingName;
    private TestDialogue actor;

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


    void Start () {
        textToDisplay = transform.FindChild("TextPanelText").GetComponent<Text>();
        textToDisplay.text = "";
        newDialogue = false;
        gameObject.SetActive(false);
        questionPanel.SetActive(false);
        talkingName = "";
    }

    public void setAnswers(Queue<string> answers)
    {
        
        questionPanel.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = answers.Dequeue();
        questionPanel.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = answers.Dequeue();
        questionPanel.SetActive(true);
        questionPanel.transform.GetChild(1).GetComponent<Button>().Select();
        questionPanel.transform.GetChild(0).GetComponent<Button>().Select();
    }

    public void sendDialogueRequest(string _dialogueID, TestDialogue _actor)
    {
        actor = _actor;
        string line;
        textList = new Queue<string>();
        talkingName = actor.getName();
        try
        {
            string path = ".\\Assets\\HomemadeAssets\\Dialogue\\" + talkingName + ".zs";
            StreamReader sr = new StreamReader(path);

            line = sr.ReadLine();
            Debug.Log(line);
            Debug.Log(_dialogueID);
            while (line != null && !line.Equals(_dialogueID)) {
                line = sr.ReadLine();
            }
            if(line == null)
            {
                Debug.Log("Dialogue not found");
                sr.Close();
                return;
            }
            line = sr.ReadLine();
            while (line != null && !line.Equals("end"))
            {
                textList.Enqueue(line);
                line = sr.ReadLine();
            }
            sr.Close();
            displayText(textList,talkingName);
        }
        catch(IOException e)
        {
            Debug.Log("Exception: " + e.Message);
        }


    }

    public void displayText(Queue<string> _textList, string _name)
    {
        textList = _textList;
        talkingName = _name;

        GameController.Instance.pauseEntities();
        newDialogue = true;
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
        else if (Input.GetButtonDown("Interact"))
        {
            displayNext();
        }
    }

    public void displayNext()
    {
        if (textList.Count == 0)
        {
            GameController.Instance.pauseEntities();
            gameObject.SetActive(false);
        }
        else
        {
            string text = textList.Dequeue();
            //Check for question with n answers
            if (text.StartsWith("@?"))
            {
                int n = text[3]-'0';
                Debug.Log(n);
                askingQuestion = true;
                textToDisplay.text = textList.Dequeue();

                Queue<string> answers = new Queue<string>();
                for (int i = 0; i < n; i++)
                {
                    answers.Enqueue(textList.Dequeue());
                }
                pointer = new string[4];
                for(int i = 0; i < n; i++)
                {
                    pointer[i] = textList.Dequeue().Substring(2);
                }
                setAnswers(answers);
            }
            //Check for pointer links
            else if (text.StartsWith("@>"))
            {
                sendDialogueRequest(text.Substring(2), actor);
            }
            //
            else if (text.StartsWith("@#"))
            {
                text = text.Substring(3);
                if (text.StartsWith("setDialogue"))
                {
                    actor.setDialogue(text.Substring(12));
                }
                if (text.StartsWith("setFriendPoints"))
                {
                    actor.setFriendPoints(int.Parse(text.Substring(16)));
                }
                displayNext();
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
        questionPanel.SetActive(false);
        Debug.Log(pointer[answer]);
        sendDialogueRequest(pointer[answer], actor);
    }

    public bool getState()
    {
        return gameObject.activeSelf;
    }
}
