using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TeleportController : MonoBehaviour {
    public static TeleportController Instance { get; set; }

    //Last clean: 31/01/2018

    [SerializeField]
    private bool init = false;
    private bool pressed;
    private int pos;
    private Dictionary<string,float[]> teleportLocations;
    private Text locationString;
    private string tp;
    private TeleportPad pad;

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
        teleportLocations = GameController.Instance.levelSystem.activeLevel.getTeleportLocations();
        tp = teleportLocations.Keys.First();
        locationString.text = tp;
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
                    pos = teleportLocations.Count - 1;
                }
            }
            else if (Input.GetAxis("LS-X") > 0.2f && pressed != true)
            {
                pos++;
                if (pos > teleportLocations.Count - 1)
                {
                    pos = 0;
                }
            }
            pressed = true;
            tp = teleportLocations.Keys.ElementAt(pos);
            updateDisplay(tp);
        }

        if (Input.GetButtonUp("B"))
        {
            closeController();
        }

        if (Input.GetButtonUp("A"))
        {
            float[] position;
            teleportLocations.TryGetValue(tp, out position);
            PlayerController.Instance.transform.position = new Vector3(position[0],position[1],position[2]) + Vector3.up;
            FollowCamera.Instance.setFollow();
            closeController();
        }
    }
}
