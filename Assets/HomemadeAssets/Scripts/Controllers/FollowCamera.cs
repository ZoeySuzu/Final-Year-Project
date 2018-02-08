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

    [SerializeField]
    private float maxHeight, maxDistance;

    private GameObject player;
    private GameObject target;
    private GameObject targetEnnemy;

    private Vector3 center;
    private Vector3 init;
    private Vector3 offset;
    private Vector3 lerpStartPos;
    private Vector3 lerpTargetPos;
    private Vector3 lerpFocus;

    private bool locked;
    private bool lerping;
    private bool lookCenter;
    private bool enemyFocused;
    private bool l2, rs;

    private float lerpStartTime;


    void Start()
    {
        player = transform.parent.gameObject;
        init = player.transform.position - transform.position;
        offset = init;
        l2 = false;
        rs = false;
        lookCenter = false;
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
        if (Input.GetAxis("L2") <= 0.2 && l2)
        {
            l2 = false;
        }
        if (Input.GetAxis("RS") <0.4 && Input.GetAxis("RS-X") > -0.4 && rs)
        {
            rs = false;
        }

        if (lerping)
        {
            float lerpTime = Time.time - lerpStartTime;
            float lerpPercent = lerpTime / 0.3f;
            if (lookCenter)
            {
                transform.position = Vector3.Lerp(lerpStartPos, player.transform.position + player.transform.forward * -5 + Vector3.up * 3 + player.transform.right * 0f, lerpPercent);
                center = player.transform.position + (targetEnnemy.transform.position - player.transform.position) / 2;
                transform.LookAt(Vector3.Lerp(lerpFocus,center,lerpPercent));
            }

            else if (lerpTargetPos == Vector3.zero)
            {
                transform.position = Vector3.Lerp(lerpStartPos, player.transform.position - transform.parent.rotation * init, lerpPercent);
                transform.LookAt(Vector3.Lerp(lerpFocus,player.transform.position,lerpPercent));
            }
            
            else
            {
                transform.position = Vector3.Lerp(lerpStartPos, lerpTargetPos, lerpPercent);
                transform.LookAt(Vector3.Lerp(lerpFocus, target.transform.position,lerpPercent));
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
                if (Input.GetAxis("RS-X") < -0.5f && !rs)
                {
                    rs = true;
                    return;
                }
                else if (Input.GetAxis("RS-X") > 0.5f && !rs)
                {
                    rs = true;
                    setFocus(PlayerController.Instance.getNextEnnemy(targetEnnemy));
                    return;
                }
                else if (Input.GetAxis("L2") > 0.2 && !l2)
                {
                    l2 = true;
                    setFollow();
                    return;
                }

                center = player.transform.position + (targetEnnemy.transform.position - player.transform.position) / 2;
                transform.position = player.transform.position + player.transform.forward * -5 + Vector3.up * 3 + player.transform.right * 0f;
                lerpFocus = center;
                transform.LookAt(center);
            }
            else
            {
                var cameraX = Input.GetAxis("RS-X") * 6;
                var cameraHeight = new Vector3(0, Input.GetAxis("RS-Y") * 0.3f, 0);
                var cameraDistance = -Input.GetAxis("RS-Y") * 0.3f;

                if (Input.GetAxis("L2") > 0.2 && !l2)
                {
                    l2 = true;
                    GameObject ennemy = PlayerController.Instance.getNearestEnnemy();
                    if (ennemy == null)
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
                    if (transform.position.y < player.transform.position.y-0.5f)
                    {
                        transform.position = new Vector3(transform.position.x, player.transform.position.y-0.5f, transform.position.z);
                    }

                    
                    transform.LookAt(player.transform);
                    transform.position += transform.forward * cameraDistance;

                    var mag = Mathf.Abs(transform.localPosition.x) + Mathf.Abs(transform.localPosition.z);

                    if (mag < 4f)
                    {
                        transform.localPosition = new Vector3(transform.localPosition.x/mag*4, transform.localPosition.y, transform.localPosition.z/mag*4);
                    }else if(mag > 15f)
                    {
                        transform.localPosition = new Vector3(transform.localPosition.x / mag * 15, transform.localPosition.y, transform.localPosition.z / mag * 15);
                    }
                    lerpFocus = player.transform.position;
                    transform.LookAt(player.transform);

                    offset = player.transform.position - transform.position;
                    
                }    
            }
        }
    }

    public void setFollow()
    {
        recenter();
        locked = false;
        enemyFocused = false;
    }

    public void setFocus(GameObject _target)
    {
        targetEnnemy = _target;
        lerpCenterStart();
        locked = false;
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

    /*
    public void setSpecific(GameObject targetA, GameObject targetB, Vector3 offset)
    {
        Vector3 middle = (targetB.transform.position - targetA.transform.position) / 2;
        transform.position = middle + targetA.transform.right * 3;
        transform.LookAt(middle);
        locked = true;
    }*/


    public void recenter()
    {
        lerpStart(Vector3.zero);
    }

    public void lerpStart(Vector3 target)
    {
        lerping = true;
        lerpStartTime = Time.time;
        lerpStartPos = transform.position;
        lerpTargetPos = target;
        lookCenter = false;
    }

    public void lerpCenterStart()
    {
        lerping = true;
        lerpStartTime = Time.time;
        lerpStartPos = transform.position;
        lookCenter = true;
    }

    public bool getLerp()
    {
        return lerping;
    }
}