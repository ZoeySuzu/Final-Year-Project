using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum directions { up, down, left, right, forward, back };

public class MovingPlatform : MonoBehaviour {

    public directions dir;
    public float distance;
    public float speed;

    private float position;
    private Vector3 vector;
    private bool backwards;
    private GameObject target;
    private Vector3 offset;

	// Use this for initialization
	void Start () {
        position = 0;
        backwards = false;

        switch(dir){
            case directions.up:
                {
                    vector = Vector3.up;
                    break;
                }
            case directions.down:
                {
                    vector = Vector3.down;
                    break;
                }
            case directions.left:
                {
                    vector = Vector3.left;
                    break;
                }
            case directions.right:
                {
                    vector = Vector3.right;
                    break;
                }
            case directions.forward:
                {
                    vector = Vector3.forward;
                    break;
                }
            case directions.back:
                {
                    vector = Vector3.back;
                    break;
                }
            
            default:
                break;
        }
	}

    public void OnTriggerStay(Collider other)
    {
        target = other.gameObject;
    }
    public void OnTriggerExit(Collider other)
    {
        target = null;
    }

    // Update is called once per frame
    void Update () {

        if (!backwards) {
            position += 1;
            transform.Translate(vector * speed * Time.deltaTime);
            if (position >= distance*10/speed)
            {
                backwards = true;
            }
        }

        if (backwards)
        {
            position -= 1;
            transform.Translate(vector * speed * Time.deltaTime *-1);
            if (position <= 0)
            {
                backwards = false;
            }
        }
        if (target != null)
        {
            if (backwards)
            {
                target.transform.Translate(-1*vector * speed * Time.deltaTime);
            }
            else
            {
                target.transform.Translate(vector * speed * Time.deltaTime);
            }
        }
    }

}
