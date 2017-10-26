using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {


    public GameObject target;

    private LineRenderer line;
    private Vector3 vector;

	// Use this for initialization
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
	
	// Update is called once per frame
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
