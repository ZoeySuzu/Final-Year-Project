using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Last clean: 29/11/2017
//Todo Implement climbing:


public class Lader : Interactable {
    
    public override void OnTriggerStay(Collider other)
    {
        
    }

    public Lader()
    {
        interaction = "Climb";
    }
}
