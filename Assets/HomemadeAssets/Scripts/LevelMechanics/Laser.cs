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


    //Set up targeting
	void Start () {
        line = GetComponent<LineRenderer>();
        if (target != null)
        {
            transform.LookAt(target.transform.position);
        }
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            LaserReactor lr = hit.collider.gameObject.GetComponent<LaserReactor>();
            if (lr != null)
            {
                lr.Trigger();
            }
            if (hit.collider != transform.parent && hit.collider.GetComponent<Laser>() == null)
            {
                line.SetPosition(1, Vector3.forward * hit.distance);
            }
        }
        else
        {
            line.SetPosition(1, Vector3.forward * 1000);
        }
    }

	

    //Check for laser interuption;
	void LateUpdate () {
        RaycastHit hit ;
        if(Physics.Raycast(transform.position,transform.forward, out hit)){
            LaserReactor lr = hit.collider.gameObject.GetComponent<LaserReactor>();
            if (lr != null)
            {
                lr.Trigger();
            }
            if (hit.collider != transform.parent && hit.collider.GetComponent<Laser>() == null)
            {
                line.SetPosition(1, Vector3.forward * hit.distance);
            }
        }
        else
        {
            line.SetPosition(1, Vector3.forward * 1000);
        }
	}
}
