using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Last clean: 29/11/2017
//to do: Fix distance/time equation

public class MovingPlatform : MonoBehaviour {

    [SerializeField]
    private directions dir;
    [SerializeField]
    private float distance;
    [SerializeField]
    private float time;
    private Cooldown cd;
    private float speed;

    private Vector3 vector;
    private GameObject target;
    private Vector3 offset;
    private List<GameObject> targets = new List<GameObject>();

    // Use this for initialization
    void Start () {
        speed = distance*-1 / time;
        cd = new Cooldown(time);
 
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
        if (!targets.Contains(other.gameObject) && !other.isTrigger)
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
        
        if (cd.ready)
        {
            StartCoroutine(cd.StartCooldown());
            speed *= -1;
        }
        transform.Translate(vector * speed * Time.deltaTime);

        foreach (GameObject target in targets)
        {
            if(target == null)
            {
                break;
            }
            target.transform.Translate(vector * speed * Time.deltaTime);
        }
    }

}
