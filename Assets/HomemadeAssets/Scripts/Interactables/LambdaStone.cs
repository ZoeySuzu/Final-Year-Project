using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LambdaStone : Interactable
{

    [SerializeField]
    private string[] buttons;

    [SerializeField]
    private string goal, instructionIN, instructionOUT, author;

	public string inCurrent { get; private set; }
    public string outCurrent { get; private set; }
    public string inPrevious { get; private set; }


    private string[] input;
    private int opNumber;
    private int opsNeeded;

    private bool finished;

    private ParticleSystem particle;
    private Material mat;

    public string[] getButtons() { return buttons;}
    public string getGoal() { return goal; }


    public LambdaStone()
    {
        interaction = "Use";
    }

    private void Start()
    {
        finished = false;

        particle = GetComponentInChildren<ParticleSystem>();
        particle.gameObject.SetActive(false);
        mat = transform.FindChild("StoneSlate").GetComponent<Renderer>().material;
        mat.color = Color.grey;

        gameUI = UIController.Instance;
        opNumber = 0;
        opsNeeded = instructionIN.Length;
        input = new string[opsNeeded];
        inCurrent = instructionIN;
        outCurrent = instructionOUT;
        inPrevious = "";
    }

    public void removeAnswer()
    {
        if (opNumber > 0)
        {
            opNumber--;
            input[opNumber] = null;
        }
    }

    public void addAnswer(string s)
    {
        if (opNumber < opsNeeded)
        {
            input[opNumber] = s;
            string activeChar = instructionIN[opNumber].ToString();
            inCurrent = inCurrent.Substring(1);
            inPrevious = inPrevious + "("+s+")";
            outCurrent = outCurrent.Replace(activeChar, s);
            opNumber++;
            
        }
        else
            Debug.Log("Can't add more input");
    }

    public void clearAnswer()
    {
        opNumber = 0;
        inCurrent = instructionIN;
        outCurrent = instructionOUT;
        inPrevious = "";
        input = new string[opsNeeded];
    }

    public void testTheory()
    {
        if (opNumber != opsNeeded)
            return;

        if(outCurrent != goal)
        {
            return;
        }
        success();
    }

    private void success()
    {
        Debug.Log("Success");
        particle.gameObject.SetActive(true);
        mat.color = Color.blue;
        finished = true;
        PuzzleController.Instance.endPuzzle();
    }

    public override void interact()
    {
        if (!PuzzleController.Instance.getActive() && finished == false)
        {
            PuzzleController.Instance.startPuzzle(this);
        }
    }
}
