using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchFire : MonoBehaviour {

    bool isLit;
    GameObject fire;
    TrailRenderer tr;

	// Use this for initialization
	void Start () {
        isLit = false;
        fire = transform.GetChild(0).gameObject;
        fire.SetActive(false);
        tr = GetComponent<TrailRenderer>();
        tr.enabled = false;
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Particle_Fire" && !isLit)
        {
            Debug.Log("Stick should Catch Fire");
            isLit = true;
            fire.SetActive(true);
            tr.enabled = true;
        }
    }
}
