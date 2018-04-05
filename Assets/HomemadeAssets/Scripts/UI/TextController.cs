using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour {
    public static TextController Instance { get; set; }

    [SerializeField]
    private GameObject questionPanel;

    private string dialogueID;
    private Text textToDisplay;
    private Queue<string> textList;
    private bool newDialogue;
    private bool askingQuestion;
    private bool complexScript;
    private string[] split;
    private StreamReader sr;

    private string[] pointer;
    private string talkingName;
    private string line;
    private NPCController actor;
    private CharacterController character;

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
        textToDisplay = transform.FindChildByRecursive("TextPanelText").GetComponent<Text>();
        textToDisplay.text = "";
        newDialogue = false;
        gameObject.SetActive(false);
        questionPanel.SetActive(false);
        talkingName = "";
    }

    private void findDialogue(string _dialogueID)
    {
        sr = openScript(); 
        
        Debug.Log("Dialogue ID: " + _dialogueID);
        dialogueID = _dialogueID;
        while (line != null && !line.Equals("&"+_dialogueID))
        {
            line = sr.ReadLine();
        }
        if (line == null)
        {
            Debug.Log("Dialogue not found");
            sr.Close();
            return;
        }
        line = sr.ReadLine();
        while (!line.Equals("&end"))
        {
            if (line != null)
            {
                textList.Enqueue(line);
            }
            line = sr.ReadLine();
        }
        sr.Close();
        displayText(textList, talkingName);
    }

    private StreamReader openScript()
    {
        try
        {
            string path = ".\\Assets\\HomemadeAssets\\Dialogue\\" + talkingName + ".txt";
            sr = new StreamReader(path);
            Debug.Log(line = sr.ReadLine());
            if (complexScript && !line.Contains("Advanced"))
            {
                Debug.Log("Expected advanced script");
            }
            else if (!complexScript && line.Contains("Advanced"))
            {
                Debug.Log("Expected simple script");
            }
            return sr;
        }
        catch (IOException e)
        {
            Debug.Log("Exception: " + e.Message);
        }
        return null;
    }

    public void sendDialogueRequest(string _dialogueID, NPCController _actor, bool _complexScript)
    {
        dialogueID = _dialogueID;
        actor = _actor;
        textList = new Queue<string>();
        talkingName = actor.getName();
        complexScript = _complexScript;
        findDialogue(_dialogueID);
    }

    public void sendDialogueRequest(string _dialogueID, CharacterController _actor, bool _complexScript)
    {
        dialogueID = _dialogueID;
        actor = _actor;
        textList = new Queue<string>();
        talkingName = actor.getName();
        complexScript = _complexScript;

        openScript();
        findDialogue(_dialogueID);
    }

    public void displayText(Queue<string> _textList, string _name)
    {
        textList = _textList;
        talkingName = _name;

        GameController.Instance.pauseEntities(true);
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
        else if (Input.GetButtonDown("A"))
        {
            displayNext();
        }
    }

    public void displayNext()
    {
        //If there is nothing left to display
        if (textList.Count == 0)
        {
            GameController.Instance.pauseEntities(false);
            gameObject.SetActive(false);
            FollowCamera.Instance.setFollow();
        }
        else
        {
            string text = textList.Dequeue();
            //Check for question with n answers
            if (text.StartsWith("@?"))
            {
                int n = text[3] - '0';
                askingQuestion = true;

                Queue<string> answers = new Queue<string>();
                for (int i = 0; i < n; i++)
                {
                    answers.Enqueue(textList.Dequeue());
                }
                pointer = new string[4];
                for (int i = 0; i < n; i++)
                {
                    pointer[i] = dialogueID + "A" + (i+1);
                }
                setAnswers(answers);
            }

            //Check for pointer links
            else if (text.StartsWith("@goto"))
            {
                findDialogue(text.Substring(6));
            }

            //check for friend point condition
            else if (text.StartsWith("@fp"))
            {
                bool higher = true;
                if (text[3] == '<') {
                    higher = false;
                }
                else
                {
                    Debug.Log("wrong comparison");
                }
                if (!character.getFP().checkFriendPoints(higher, int.Parse(text.Substring(4))))
                {
                    while (text != null && !text.Equals("else"))
                    {
                        text = textList.Dequeue();
                    }
                }
                displayNext();
            }

            //check for script called methods
            else if (text.StartsWith("@#"))
            {
                split = text.Split();
                if (split[1] == "setDialogue")
                {
                    actor.setDialogue(split[2]);
                }
                else if (split[1] == ("setFriendPoints"))
                {
                    character.updateFP(int.Parse(split[2]));
                }
                else if (split[1] == ("setCamera"))
                {
                    setCamera(split[2]);
                }
                displayNext();
            }

            else if (text.StartsWith("else") || text.StartsWith(" "))
            {
                return;
            }
            else
            {
                textToDisplay.text = text;
            }
        }
    }

    //React to answer
    public void answer(int answer)
    {
        askingQuestion = false;
        questionPanel.SetActive(false);
        findDialogue(pointer[answer]);
    }

    //get methods
    public bool getState()
    {
        return gameObject.activeSelf;
    }

    //set methods
    //Get script camera info and make it move
    private void setCamera(string text)
    {
        var list = GameController.Instance.cameraIds;
        GameObject go = null;
        Vector3 vec = Vector3.zero;
        foreach(CameraID cam in list)
        {
            if (cam.getId() == text)
            {
                go = cam.gameObject;
                vec = cam.getVec();
                break;
            }
        }
        if (go == null)
        {
            Debug.Log(text + " Object not found");
            return;
        }
        FollowCamera.Instance.setSpecific(go, vec);
    }

    //Display answers (implement more than 2 options)
    private void setAnswers(Queue<string> answers)
    {
        for (int i = 0; i < 4; i++)
        {
            if (answers.Count != 0)
            {
                questionPanel.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = answers.Dequeue();
                questionPanel.transform.GetChild(i).GetComponent<Button>().interactable = true;
            }
            else
            {
                questionPanel.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = "";
                questionPanel.transform.GetChild(i).GetComponent<Button>().interactable = false;
            }

        }
        questionPanel.SetActive(true);
        questionPanel.transform.GetChild(1).GetComponent<Button>().Select();
        questionPanel.transform.GetChild(0).GetComponent<Button>().Select();
    }
}
