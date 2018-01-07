﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleController : MonoBehaviour {
    public static PuzzleController Instance { get; set; }

    [SerializeField]
    private Text goalString, inString, outString,answerString,resultString;

    private Transform buttons;
    private LambdaStone stone;
    private bool active, enable;

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
        enable = false;
        goalString.text = "";
        inString.text = "";
        outString.text = "";
        gameObject.SetActive(false);
        buttons = transform.FindChild("Puzzle_ButtonPanel");
    }

    public void endPuzzle()
    {
        active = false;
        GameController.Instance.pauseEntities();
        gameObject.SetActive(false);
        enable = false;
    }

    public void startPuzzle(LambdaStone _stone)
    {
        active = true;
        stone = _stone;
        goalString.text = stone.getGoal();
        inString.text = stone.getInstrucionIN();
        outString.text = stone.getInstrucionOUT();
        buttonList = stone.getButtons();
        answerString.text = "";
        resultString.text = "";

        for (int i = 0; i<5; i++)
        {
            if (i >= buttonList.Length)
            {
                buttons.GetChild(i).GetChild(0).GetComponent<Text>().text = "";
                buttons.GetChild(i).GetComponent<Button>().interactable = false;
            }
            else
            {
                buttons.GetChild(i).GetChild(0).GetComponent<Text>().text = buttonList[i];
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
        answerString.text = "";
        stone.clearAnswer();
    }

    public void removeAnswer()
    { 
        stone.removeAnswer();
        answerString.text = stone.getAnswer();
    }

    public void testAnswer()
    {
        stone.testTheory();
        resultString.text = stone.getResult();
    }
	
    public bool getActive()
    {
        return active;
    }

    public void addAwnser(int i)
    {
        if (enable)
        {
            stone.addAnswer(buttonList[i]);
            answerString.text = stone.getAnswer().ToString();
        }
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("B"))
        {
            endPuzzle();
        }
        if(active == true && Input.GetButtonUp("A"))
        {
            enable = true;
        }

    }
}
