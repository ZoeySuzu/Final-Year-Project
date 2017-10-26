using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchReactor : MonoBehaviour {

    public bool activeOnStart;

	// Use this for initialization
	void Start () {
        gameObject.SetActive(activeOnStart);
	}
	
    public void switchOn()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void switchOff()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }


	// Update is called once per frame
	void Update () {
		
	}
}
