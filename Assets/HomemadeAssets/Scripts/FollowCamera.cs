using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Last clean: 29/11/2017

public class FollowCamera : MonoBehaviour
{
    public static FollowCamera Instance { get; set; }

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

    private GameObject player;
    private GameObject target;

    private GameObject targetEnnemy;
    private bool enemyFocused;
    private Vector3 center;

    public float maxHeight;
    public float maxDistance;

    private Vector3 init;
    private Vector3 offset;

    private bool locked;
    private bool lerping;
    private float lerpStartTime;
    private Vector3 lerpStartPos;
    private Vector3 lerpTargetPos;

    void Start()
    {
        player = transform.parent.gameObject;
        init = player.transform.position - transform.position;
        offset = init;
    }

    public GameObject targetedEnnemy()
    {
        if (enemyFocused)
        {
            return targetEnnemy;
        }
        return null;
    }

    void LateUpdate()
    {
        if (lerping)
        {
            float lerpTime = Time.time - lerpStartTime;
            float lerpPercent = lerpTime / 0.3f;
            if (lerpTargetPos == Vector3.zero)
            {
                transform.position = Vector3.Lerp(lerpStartPos, player.transform.position - transform.parent.rotation * init, lerpPercent);
                transform.LookAt(player.transform);
            }
            else
            {
                transform.position = Vector3.Lerp(lerpStartPos, lerpTargetPos, lerpPercent);
                transform.LookAt(target.transform);
            }
            
            if (lerpPercent >= 1.0f)
            {
                lerping = false;
                offset = player.transform.position - transform.position;
            }
        }
        else if (locked)
        {
            return;
        }
        else {
            if (enemyFocused)
            {
                if (Input.GetButtonDown("L1"))
                {
                    setFollow();
                }
                center = player.transform.position + (targetEnnemy.transform.position - player.transform.position) / 2;
                transform.position = player.transform.position + (player.transform.position - targetEnnemy.transform.position) / 2 + Vector3.up * 5;
                transform.LookAt(center);
            }
            else
            {
                var cameraX = Input.GetAxis("RS-X") * 6;
                var cameraHeight = new Vector3(0, Input.GetAxis("RS-Y") * 0.3f, 0);
                //var cameraDistance = -Input.GetAxis("Joystick_B Y") * 0.1f;

                if (Input.GetButtonDown("L1"))
                {
                    GameObject ennemy = PlayerController.Instance.getNearestEnnemy();
                    if(ennemy == null)
                    {
                        setFollow();
                    }
                    else
                    {
                        setFocus(ennemy);
                    }

                }

                
                else
                {
                    Quaternion rotation = Quaternion.Euler(0, cameraX, 0);
                    transform.position = player.transform.position - (rotation * offset) + cameraHeight;
                    if (transform.position.y > player.transform.position.y + maxHeight)
                    {
                        transform.position = new Vector3(transform.position.x, player.transform.position.y + maxHeight, transform.position.z);
                    }
                    if (transform.position.y < player.transform.position.y)
                    {
                        transform.position = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
                    }

                    offset = player.transform.position - transform.position;
                }
                transform.LookAt(player.transform);
            }
        }
    }

    public void setFollow()
    {
        offset = player.transform.position - transform.parent.rotation * init;
        recenter();
        locked = false;
        enemyFocused = false;
    }

    public void setFocus(GameObject target)
    {
        targetEnnemy = target;
        enemyFocused = true;
    }

    public GameObject getFocus()
    {
        if (enemyFocused)
            return targetEnnemy;
        else
            return null;
    }

    public void setSpecific(GameObject _target, Vector3 offset)
    {
        target = _target;
        lerpStart(target.transform.position+offset);
        locked = true;
    }

    public void setSpecific(GameObject targetA, GameObject targetB, Vector3 offset)
    {
        Vector3 middle = (targetB.transform.position - targetA.transform.position) / 2;
        transform.position = middle + targetA.transform.right * 3;
        transform.LookAt(middle);
        locked = true;
    }


    public void recenter()
    {
        lerping = true;
        lerpStartTime = Time.time;
        lerpStartPos = transform.position;
        lerpTargetPos = Vector3.zero;
    }

    public void lerpStart(Vector3 target)
    {
        lerping = true;
        lerpStartTime = Time.time;
        lerpStartPos = transform.position;
        lerpTargetPos = target;
    }

    public bool getLerp()
    {
        return lerping;
    }
}