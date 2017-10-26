using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lader : Interactable {

    void Start()
    {
        gameUI = GetComponentInParent<UIController>();
    }

    public override void OnTriggerStay(Collider other)
    {
        
    }

    public Lader()
    {
        interaction = "Climb";
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
