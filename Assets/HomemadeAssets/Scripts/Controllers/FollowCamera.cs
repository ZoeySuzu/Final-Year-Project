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
    private Vector3 target;
    private GameObject targetEnemy;

    private Vector3 center;
    private Vector3 init;
    private Vector3 offset;
    private Vector3 lerpStartPos;
    private Vector3 lerpTargetPos;
    private Vector3 lerpFocus;
    private Vector3 aimOffset;

    private bool locked;
    private bool lerping;
    private string lookAt;
    private bool enemyFocused;
    private bool l2, rs;
    private bool aim;

    private float lerpStartTime;


    void Start()
    {
        player = transform.parent.gameObject;
        init = player.transform.position - transform.position;
        offset = init;
        l2 = false;
        rs = false;
        lookAt = "Default";
        aim = false;
    }

    public GameObject targetedEnnemy()
    {
        if (enemyFocused)
        {
            return targetEnemy;
        }
        return null;
    }

    void LateUpdate()
    {
        checkAim();
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
            if (lookAt == "Center")
            {
                transform.position = Vector3.Lerp(lerpStartPos, player.transform.position + player.transform.forward * -5 + Vector3.up * 3, lerpPercent);
                center = player.transform.position + (targetEnemy.transform.position - player.transform.position) / 2;
                transform.LookAt(Vector3.Lerp(lerpFocus,center,lerpPercent));
            }
            else if (lookAt == "Aim")
            {
                var ab = transform.parent.rotation * init/2;
                var bc = player.transform.right + player.transform.up * 0.5f + player.transform.forward * -1;
                transform.position = Vector3.Lerp(lerpStartPos, player.transform.position-ab+bc, lerpPercent);
                transform.LookAt(Vector3.Lerp(lerpFocus, player.transform.position - ab + bc + ab, lerpPercent));

            }
            else if (lookAt == "Player")
            {
                transform.position = Vector3.Lerp(lerpStartPos, player.transform.position - transform.parent.rotation * init, lerpPercent*1.5f);
                transform.LookAt(Vector3.Lerp(lerpFocus,player.transform.position,lerpPercent));
            }
            
            else
            {
                transform.position = Vector3.Lerp(lerpStartPos, lerpTargetPos, lerpPercent);
                transform.LookAt(Vector3.Lerp(lerpFocus, target,lerpPercent));
            }
            
            if (lerpPercent >= 1.0f)
            {
                lerping = false;
                if (!aim)
                {
                    offset = player.transform.position - transform.position;
                }
                else
                {
                    offset = transform.parent.rotation * init / 2;
                }
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
                    GameObject nextEnnemy = PlayerController.Instance.getNextEnnemy(targetEnemy);
                    if(nextEnnemy != null)
                        setFocus(nextEnnemy);
                    return;
                }
                else if (Input.GetAxis("L2") > 0.2 && !l2)
                {
                    l2 = true;
                    setFollow();
                    return;
                }

                var mag = Mathf.Abs(targetEnemy.transform.position.x -transform.position.x) + Mathf.Abs(targetEnemy.transform.position.z - transform.position.z);
                if (mag > 30f)
                {
                    setFollow();
                    return;
                }

                center = player.transform.position + (targetEnemy.transform.position - player.transform.position) / 2;
                transform.position = player.transform.position + player.transform.forward * -5 + Vector3.up * 3 + aimOffset;
                lerpFocus = center;
                transform.LookAt(center);
            }
            else
            {
                var cameraX = Input.GetAxis("RS-X") * 6;
                var cameraHeight = new Vector3(0, Input.GetAxis("RS-Y") * 0.3f, 0);
                if (aim)
                {
                    cameraHeight /= 2;
                }

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
                    if (transform.position.y < player.transform.position.y - 0.8f)
                    {
                        transform.position = new Vector3(transform.position.x, player.transform.position.y - 0.8f, transform.position.z);
                    }

                    
                    transform.LookAt(player.transform);
                    if (!aim)
                    {
                        transform.position += transform.forward * cameraDistance;

                        var mag = Mathf.Abs(transform.localPosition.x) + Mathf.Abs(transform.localPosition.z);

                        if (mag < 4f)
                        {
                            transform.localPosition = new Vector3(transform.localPosition.x / mag * 4, transform.localPosition.y, transform.localPosition.z / mag * 4);
                        }
                        else if (mag > 15f)
                        {
                            transform.localPosition = new Vector3(transform.localPosition.x / mag * 15, transform.localPosition.y, transform.localPosition.z / mag * 15);
                        }
                    }
                    offset = player.transform.position - transform.position;
                    if (aim)
                    {
                        var ab = transform.parent.position - transform.position;
                        transform.position += player.transform.right + player.transform.up * 0.5f+ player.transform.forward *-1;
                        lerpFocus = transform.position + ab;
                        transform.LookAt(lerpFocus);
                    }
                    else
                    { 
                        lerpFocus = player.transform.position;
                        transform.LookAt(player.transform.position);
                    }
                   

                }    
            }
        }
    }

    public void setAim(bool value)
    {
        aim = value;
        if (aim) {
            lerpStart(Vector3.zero, "Aim");
        }
        else
        {
            recenter();
        }
    }

    public bool getAim()
    {
        return aim;
    }

    private void checkAim() {
        if (aim)
        {
            aimOffset = transform.up + transform.right;
        }
        else
        {
            aimOffset = Vector3.zero;
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
        targetEnemy = _target;
        lerpStart(Vector3.zero, "Center");
        locked = false;
        enemyFocused = true;
    }

    public GameObject getFocus()
    {
        if (enemyFocused)
            return targetEnemy;
        else
            return null;
    }

    public void setSpecific(GameObject _target, Vector3 offset)
    {
        target = _target.transform.position;
        lerpStart(target+offset,"Default");
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


    private void recenter()
    {
        lerpStart(Vector3.zero,"Player");
    }

    public void lerpStart(Vector3 _target, string _lookAt)
    {
        lookAt = _lookAt;
        lerping = true;
        lerpStartTime = Time.time;
        lerpStartPos = transform.position;
        lerpTargetPos = _target;
    }

    public bool getLerp()
    {
        return lerping;
    }
}