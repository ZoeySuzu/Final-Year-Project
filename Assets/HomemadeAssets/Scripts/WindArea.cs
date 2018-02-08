using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Cleanup: 08/01/2018
//Todo: Add directional support;

public class WindArea : MonoBehaviour {

    Rigidbody rb;

    //Lift rigidbodys up or move boxes manually
    private void OnTriggerStay(Collider other)
    {
        if (!other.isTrigger)
        {
            var a = GetComponent<CapsuleCollider>();
            var origin = transform.position.y;
            var target = other.transform.position.y - other.transform.localScale.y / 2;
            var distance = target - origin;

            var percentForce = 1-(distance / a.height);
            if (percentForce < 0)
                percentForce = 0;

            if ((rb = other.GetComponent<Rigidbody>()) != null)
            {
                //rb.velocity = new Vector3(0,rb.velocity.y + 26*percentForce*rb.mass, 0);
                rb.AddForce(Vector3.up * 18 * percentForce * rb.mass);
            }

        }
    }
}
