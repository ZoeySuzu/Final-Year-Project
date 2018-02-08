using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {

    private Vector3 dir;
    private float scaleRatio;

    public void Start()
    {
        dir = transform.up*10;
        scaleRatio = 0;
        StartCoroutine(Explode());
    }

    // Update is called once per frame
    void Update () {
        transform.position += dir * Time.deltaTime;
        transform.localScale += Vector3.one * scaleRatio * Time.deltaTime;
	}

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        if (other.gameObject.tag == "Explodable")
        {
            Destroy(other.gameObject);
        }
    }

    IEnumerator Explode()
    {
        yield return new WaitForSeconds(0.25f);
        dir = Vector3.zero;
        yield return new WaitForSeconds(0.10f);
        scaleRatio = 15f;
        yield return new WaitForSeconds(0.25f);
        Destroy(gameObject);
    }
}
