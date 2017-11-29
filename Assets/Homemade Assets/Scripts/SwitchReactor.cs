using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Last clean: 29/11/2017

public class SwitchReactor : MonoBehaviour {

    [SerializeField]
    private bool activeOnStart;

	// Use this for initialization
	void Start () {
        gameObject.SetActive(activeOnStart);
	}
	
    public void switchActive()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void switchEnabled()
    {
        enabled = !enabled;
    }
}
