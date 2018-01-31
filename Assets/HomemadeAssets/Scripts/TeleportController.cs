using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeleportController : MonoBehaviour {
    public static TeleportController Instance { get; set; }


    bool pressed, init;
    int pos;
    ArrayList list;
    Text locationString;
    TeleportPad tp;
    TeleportPad pad;

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

    // Use this for initialization
    void Start () {
        locationString = transform.GetComponentInChildren<Text>();
        gameObject.SetActive(false);
    }

    public void openController(TeleportPad p)
    {
        init = false;
        pad = p;
        GameController.Instance.pauseEntities(true);
        pressed = false;
        pos = 0;
        list = GameController.Instance.getTeleportPads();
        tp = (TeleportPad)list[0];
        locationString.text = tp.getName();
    }

    private void closeController()
    {
        GameController.Instance.pauseEntities(false);
        gameObject.SetActive(false);
        pad.closePad();
    }

    private void updateDisplay(string s)
    {
        locationString.text = s;
    }
	
	// Update is called once per frame
	void Update () {

        if (pressed &&  (0.2f >= Input.GetAxis("LS-X") && Input.GetAxis("LS-X") >= -0.2f))
        {
            pressed = false;
        }
        else if (0.2f < Input.GetAxis("LS-X") || Input.GetAxis("LS-X") < -0.2f)
        {
            if (Input.GetAxis("LS-X") < -0.2f && pressed != true)
            {
                pos--;
                if (pos < 0)
                {
                    pos = list.Count - 1;
                }
            }
            else if (Input.GetAxis("LS-X") > 0.2f && pressed != true)
            {
                pos++;
                if (pos > list.Count - 1)
                {
                    pos = 0;
                }
            }
            pressed = true;
            tp = (TeleportPad)list[pos];
            string location = tp.getName();

            updateDisplay(location);
        }

        if (Input.GetButtonUp("A"))
        {
            if (init)
            {
                PlayerController.Instance.transform.position = tp.transform.position + Vector3.up;
                PlayerController.Instance.transform.GetChild(0).rotation = tp.transform.rotation;
                FollowCamera.Instance.setFollow();
                closeController();
            }
            else
            {
                init = true;
            }
        }
    }
}
