using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionController : MonoBehaviour {

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void setAwnsers(string awnserA, string awnserB)
    {
        transform.GetChild(0).GetChild(0).GetComponent<Text>().text = awnserA;
        transform.GetChild(1).GetChild(0).GetComponent<Text>().text = awnserB;
        gameObject.SetActive(true);
        transform.GetChild(1).GetComponent<Button>().Select();
        transform.GetChild(0).GetComponent<Button>().Select();
    }
	
    public void awnserA()
    {
        TextController.Instance.answer(0);
        close();
    }

    public void awnserB()
    {
        TextController.Instance.answer(1);
        close();
    }

    private void close()
    {
        TextController.Instance.displayNext();
        gameObject.SetActive(false);
    }
}
