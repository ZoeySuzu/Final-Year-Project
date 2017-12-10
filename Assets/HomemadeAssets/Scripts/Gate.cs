using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Last clean: 29/11/2017
//To do: Implement opening and closing

public class Gate : MonoBehaviour {

    [SerializeField]
    private Gate targetDestination;

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Object_Player")
        {
            other.transform.position = targetDestination.transform.position + targetDestination.transform.forward * 2;
            PlayerController.Instance.setSpawn(targetDestination.transform.position + targetDestination.transform.forward * 2);
        }
    }
}
