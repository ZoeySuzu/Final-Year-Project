using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTrigger : MonoBehaviour
{


    private bool isLit;
    private void Start()
    {
        isLit = false;
    }

    public bool getIsLit()
    {
        return isLit;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision with: " + other);
        if (other.GetComponentInChildren<Light>() != null)
        {
            isLit = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInChildren<Light>() != null)
        {
            isLit = false;
        }
    }

}
