using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LambdaStone : Interactable
{

    [SerializeField]
    private string[] input;
    [SerializeField]
    private string goal, instructionIN, instructionOUT, result;

    private string[] answer;
    private int opNumber;
    private int opsNeeded;
    private string activeChar;

    private bool finished;

    private ParticleSystem particle;
    private Material mat;

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
        Debug.Log("Lambda test");
        opNumber = 0;
        opsNeeded = instructionIN.Length;
        answer = new string[opsNeeded];

    }

    public string getAnswer()
    {
        return string.Join(" ",answer);
    }

    public string getResult()
    {
        return result;
    }

    public string[] getButtons()
    {
        return input;
    }

    public string getGoal()
    {
        return goal;
    }

    public string getInstrucionIN()
    {
        return instructionIN;
    }

    public string getInstrucionOUT()
    {
        return instructionOUT;
    }

    public void removeAnswer()
    {
        if (opNumber > 0)
        {
            opNumber--;
            answer[opNumber] = null;
        }
    }

    public void addAnswer(string s)
    {
        if (opNumber < opsNeeded)
        {
            answer[opNumber] = s;
            opNumber++;
        }
        else
            Debug.Log("Can't add more input");
    }

    public void clearAnswer()
    {
        opNumber = 0;
        answer = new string[opsNeeded];
    }

    public void testTheory()
    {
        if (opNumber != opsNeeded)
            return;

        result = instructionOUT;
        for (int i = 0; i < opsNeeded; i++)
        {
            activeChar = instructionIN[i].ToString();
            result = result.Replace(activeChar, answer[i]);
        }

        for (int j = 0; j < goal.Length; j++)
        {
            Debug.Log(goal[j] + " " + result[j]);
            if (goal[j] != result[j])
            {
                Debug.Log("Fail");
                return;
            }
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
