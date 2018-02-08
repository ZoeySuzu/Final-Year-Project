using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    [SerializeField]
    int maxHp; 
    int hp;

    Rigidbody rb;
    Material mat;
    Color defaultColor;
    EnemyAIType aiType = EnemyAIType.basic;

	// Use this for initialization
	void Start () {
        mat = GetComponent<Renderer>().material;
        defaultColor = mat.color;
        hp = maxHp ;
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void knockback(Vector3 force)
    {
        rb.AddForce(Vector3.up*force.y*100);
        StartCoroutine(pushBack(new Vector3(force.x, 0, force.z)));
    }

    public void setHealth(int value)
    {
        hp = hp + value;
        StartCoroutine(takeDamage());
        if (hp < 0)
        {
            onDeath();
        }
    }

    public void onDeath()
    {
        StartCoroutine(die());
    }

    IEnumerator pushBack(Vector3 force)
    {
        for (int i = 0; i <= 30; i++)
        {
            transform.position = transform.position + force * Time.deltaTime;
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator takeDamage()
    {
        for (int i = 0; i <= 4; i++)
        {
            mat.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            mat.color = defaultColor;
            yield return new WaitForSeconds(0.1f);
        }
    }
    IEnumerator die()
    {
        defaultColor = Color.black;
        yield return new WaitForSeconds(1f);
        if (FollowCamera.Instance.getFocus() == gameObject)
        {
            FollowCamera.Instance.setFollow();
        }
        Destroy(gameObject);
    }

}
