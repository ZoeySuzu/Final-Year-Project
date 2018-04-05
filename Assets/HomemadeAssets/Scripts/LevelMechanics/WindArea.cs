using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Cleanup: 08/01/2018
//Todo: Add directional support;

public class WindArea : MonoBehaviour {

    Rigidbody rb;

    [SerializeField]
    private bool sideways;

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

            if ((rb = other.GetComponentInParent<Rigidbody>()) != null)
            {
                if(!sideways)
                    rb.AddForce(Vector3.up * 18 * percentForce * rb.mass);
                else
                {
                    percentForce = 1 - (Vector3.Magnitude(other.transform.position - transform.position) / a.height);
                    other.transform.position += transform.up *percentForce* Time.deltaTime*14;
                }
            }

        }
    }
}
