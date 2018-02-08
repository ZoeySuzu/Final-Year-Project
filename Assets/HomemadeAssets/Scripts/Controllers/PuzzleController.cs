using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleController : MonoBehaviour {
    public static PuzzleController Instance { get; set; }

    [SerializeField]
    private Text goalString =null, inString = null, outString = null;

    private Transform buttons;
    private LambdaStone stone;
    private bool active;

    private string[] buttonList;


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
        active = false;
        gameObject.SetActive(false);
        buttons = transform.FindChild("Puzzle_ButtonPanel");
    }

    public void endPuzzle()
    {
        active = false;
        GameController.Instance.pauseEntities();
        gameObject.SetActive(false);
    }

    public void startPuzzle(LambdaStone _stone)
    {
        active = true;
        stone = _stone;
        goalString.text = stone.getGoal();
        goalString.color = new Color32(214,38 ,38,255);
        updateInOut();
        buttonList = stone.getButtons();

        for (int i = 0; i<6; i++)
        {
            if (i >= buttonList.Length)
            {
                buttons.GetChild(i).GetChild(0).GetComponent<Text>().text = "";
                buttons.GetChild(i).GetComponent<Button>().interactable = false;
            }
            else
            {
                var button = buttons.GetChild(i).GetChild(0).GetComponent<Text>();
                button.text = buttonList[i];
                button.fontSize = 28;
                if (button.text.Length == 1)
                {
                    button.color = new Color32(214, 105, 105, 255);
                }
                else
                {
                    button.color = Color.blue;
                }
                buttons.GetChild(i).GetComponent<Button>().interactable = true;
            }
        }

        GameController.Instance.pauseEntities();
        gameObject.SetActive(true);
        buttons.GetChild(1).GetComponent<Button>().Select();
        buttons.GetChild(0).GetComponent<Button>().Select();
    }

    public void clearAnswer()
    {
        stone.clearAnswer();
        updateInOut();
    }

    public void removeAnswer()
    { 
        stone.removeAnswer();
    }

    public void testAnswer()
    {
        stone.testTheory();
    }
	
    public bool getActive()
    {
        return active;
    }

    public void addAwnser(int i)
    {
        stone.addAnswer(buttonList[i]);
        updateInOut();
    }

    private void updateInOut()
    {
        inString.text = stone.getInstrucionIN();
        outString.text = stone.getInstrucionOUT();
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("B"))
        {
            endPuzzle();
        }

    }
}
