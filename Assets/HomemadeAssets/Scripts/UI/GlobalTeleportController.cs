using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GlobalTeleportController : MonoBehaviour
{
    public static GlobalTeleportController Instance { get; set; }

    //Last clean: 31/01/2018

    [SerializeField]
    private bool init = false;
    private bool pressed;
    private int pos;
    private Text locationString;
    private List<Level> levels;
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
    void Start()
    {
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
        levels = GameController.Instance.levelSystem.levels;
        tp = levels[0].name;
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
    void Update()
    {

        if (pressed && (0.2f >= Input.GetAxis("LS-X") && Input.GetAxis("LS-X") >= -0.2f))
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
                    pos = levels.Count - 1;
                }
            }
            else if (Input.GetAxis("LS-X") > 0.2f && pressed != true)
            {
                pos++;
                if (pos > levels.Count - 1)
                {
                    pos = 0;
                }
            }
            pressed = true;
            tp = levels[pos].name;
            updateDisplay(tp);
        }

        if (Input.GetButtonUp("B"))
        {
            closeController();
        }

        if (Input.GetButtonUp("A"))
        {
            closeController();
            FollowCamera.Instance.setFollow();
            GameController.Instance.levelSystem.loadLevel(levels[pos]);
        }
    }
}
