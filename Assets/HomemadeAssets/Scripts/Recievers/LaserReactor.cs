using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserReactor : MonoBehaviour {

    [SerializeField]
    private GameObject targetObject;

    [SerializeField]
    private bool activeOnstart;
    private bool active;


    private void Start()
    {
        active = activeOnstart;
        targetObject.SetActive(active);
    }

    public void Update()
    {
        active = activeOnstart;
        targetObject.SetActive(active);
    }

    public void Trigger()
    {
        active = !activeOnstart;
        targetObject.SetActive(active);
    }
}
