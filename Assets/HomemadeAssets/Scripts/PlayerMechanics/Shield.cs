using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {


    float scale;
    bool block;
    int strength;

	// Use this for initialization
	void Start () {
        scale = 10f;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = PlayerController.Instance.transform.position;
        StartCoroutine(startShield());
        transform.localScale += Vector3.one * scale * Time.deltaTime;
	}

    IEnumerator startShield()
    {
        yield return new WaitForSeconds(0.1f);
        block = true;
        scale = 0f;
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

}
