using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Relationship : MonoBehaviour {
    public static UI_Relationship Instance { get; set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    GameObject ab, ac, ad;
    CharacterController b, c, d;

    void Start()
    {
        

        ab = transform.FindChild("AB").gameObject;
        ac = transform.FindChild("AC").gameObject;
        ad = transform.FindChild("AD").gameObject;
    }

    void updateDisplay()
    {

    }

}
