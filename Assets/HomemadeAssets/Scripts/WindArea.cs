using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Cleanup: 08/01/2018
//Todo: Add directional support;

public class WindArea : MonoBehaviour {

    Rigidbody rb;

    //Minimise the falling force of rigidbodys
    private void OnTriggerEnter(Collider other)
    {
        if ((rb = other.GetComponent<Rigidbody>()) != null)
            rb.AddForce(Vector3.up * 200);
    }

    //Lift rigidbodys up or move boxes manually
    private void OnTriggerStay(Collider other)
    {
        if (!other.isTrigger)
        {
            if ((rb = other.GetComponent<Rigidbody>())!= null)
                rb.AddForce(Vector3.up * 50);

            if (other.GetComponent<Box>())
                other.GetComponent<Box>().boostVSpeed();
        }
    }
}
