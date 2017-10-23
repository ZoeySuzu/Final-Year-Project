using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorController : MonoBehaviour {

	// Update is called once per frame
	void Update () {

        transform.position = transform.parent.position + Vector3.up*2;
        transform.LookAt(Camera.main.transform.position);
    }
}
