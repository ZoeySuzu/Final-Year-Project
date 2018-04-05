using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Last clean: 29/11/2017
//To do: Apear relative to object height

public class IndicatorController : MonoBehaviour {

	// Update is called once per frame
	void Update () {
        transform.position = transform.parent.position + (transform.parent.localScale.y+1.0f)*Vector3.up;
        transform.LookAt(Camera.main.transform.position);
    }
}
