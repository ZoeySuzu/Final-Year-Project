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
                    vector = transform.up;
                    break;
                }
            case directions.down:
                {
                    vector = -transform.up;
                    break;
                }
            case directions.forward:
                {
                    vector = transform.forward;
                    break;
                }
            case directions.back:
                {
                    vector = -transform.forward;
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
        transform.position += (vector * speed * Time.deltaTime);

        foreach (GameObject target in targets)
        {
            if(target == null)
            {
                break;
            }
            target.transform.position += (vector * speed * Time.deltaTime);
        }
    }

}
