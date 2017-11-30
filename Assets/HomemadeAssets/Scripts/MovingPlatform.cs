using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Last clean: 29/11/2017
//to do: Fix distance/time equation

public class MovingPlatform : MonoBehaviour {

    private enum directions { up, down, left, right, forward, back };
    [SerializeField]
    private directions dir;
    [SerializeField]
    private float distance;
    [SerializeField]
    private float speed;

    private float position;
    private Vector3 vector;
    private bool backwards;

    private GameObject target;
    private Vector3 offset;
    private List<GameObject> targets = new List<GameObject>();

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
                Debug.Log("Direction for moving platform not set");
                break;
        }
	}

    public void OnTriggerStay(Collider other)
    {
        if (!targets.Contains(other.gameObject))
        {
            targets.Add(other.gameObject);
        }
    }
    public void OnTriggerExit(Collider other)
    {
        targets.Remove(other.gameObject);
    }

    // Update is called once per frame
    void Update () {

        if(Time.timeScale <= 0.01f)
        {
            return;
        }

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

        foreach (GameObject target in targets)
        {

            if(target == null)
            {
                break;
            }

            if (backwards)
            {
                target.transform.Translate(-1 * vector * speed * Time.deltaTime);
            }
            else
            {
                target.transform.Translate(vector * speed * Time.deltaTime);
            }
        }
    }

}
