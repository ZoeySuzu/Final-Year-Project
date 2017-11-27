using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour {

    public Gate targetDestination;

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Object_Player")
        {
            other.transform.position = targetDestination.transform.position + targetDestination.transform.forward * 2;
        }
    }
}
