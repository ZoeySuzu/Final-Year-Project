using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Last clean: 29/11/2017
//To do: Implement method to change target

public class Laser : MonoBehaviour {

    //Target for laser
    [SerializeField]
    private GameObject target;

    //Private variables
    private LineRenderer line;
    private Vector3 vector;


    //Set up targeting
	void Start () {
        line = GetComponent<LineRenderer>();
        if (target != null)
        {
            vector = Vector3.Normalize(target.transform.position - transform.position);
        }
        else
        {
            vector = Vector3.forward;
        }
	}
	

    //Check for laser interuption;
	void Update () {
        RaycastHit hit ;
        if(Physics.Raycast(transform.position,vector, out hit)){
            if (hit.collider != transform.parent)
            {
                line.SetPosition(1, vector * hit.distance);
            }
        }
        else
        {
            line.SetPosition(1, vector * 5000);
        }
	}
}
