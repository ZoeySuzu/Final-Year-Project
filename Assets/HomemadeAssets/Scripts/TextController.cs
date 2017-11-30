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
    private bool dialogueLeft;
    private bool askingQuestion;

    private string[] pointer;
    private string talkingName;

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

    public void sendDialogueRequest(string _dialogueID, string _name)
    {
        string line;
        textList = new Queue<string>();
        try
        {
            string path = ".\\Assets\\HomemadeAssets\\Dialogue\\" + _name + ".zs";
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
            displayText(textList,_name);
        }
        catch(IOException e)
        {
            Debug.Log("Exception: " + e.Message);
        }


    }

    public void displayText(Queue<string> text, string _name)
    {
        talkingName = _name;
        if (textList.Count > 1)
        {
            dialogueLeft = true;
        }
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
        else if (dialogueLeft && Input.GetButtonDown("Interact"))
        {
            displayNext();
        }
    }

    public void displayNext()
    {
        if (textList.Count < 1)
        {
            dialogueLeft = false;
            GameController.Instance.pauseEntities();
            gameObject.SetActive(false);
        }
        else
        {
            string text = textList.Dequeue();
            if (text.StartsWith("Question"))
            {
                int n = text[text.Length - 1] -'0';
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
                    pointer[i] = textList.Dequeue().Substring(1);
                }
                setAnswers(answers);
            }
            else if (text.StartsWith(">"))
            {
                sendDialogueRequest(text.Substring(1), talkingName);
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
        sendDialogueRequest(pointer[answer], talkingName);
    }

    public bool getState()
    {
        return gameObject.activeSelf;
    }
}
