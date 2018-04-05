using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {

    private Vector3 dir;
    private float scaleRatio;

    public void Start()
    {
        GetComponent<SphereCollider>().enabled = false;
        dir = transform.up*25;
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
        else if (other.tag == "Enemy")
        {
            EnemyController ec = other.GetComponentInParent<EnemyController>();
            ec.knockback((ec.transform.position - transform.position).normalized * 10f);
            ec.setHealth(-10);
        }
        else if (other.tag == "Player")
        {
            StartCoroutine(PlayerController.Instance.PushBack((PlayerController.Instance.transform.position - transform.position).normalized * 10f));
            PlayerController.Instance.setHealth(-10);
        }
    }

    IEnumerator Explode()
    {
        yield return new WaitForSeconds(0.10f);
        dir = Vector3.zero;
        yield return new WaitForSeconds(0.10f);
        GetComponent<SphereCollider>().enabled = true;
        transform.GetChild(0).gameObject.SetActive(true);
        scaleRatio = 15f;
        yield return new WaitForSeconds(0.25f);
        Destroy(gameObject);
    }
}
