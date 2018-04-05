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

    private string lookAt;
    private bool enemyFocused;
    private bool l2 = false, rs = false;
    private float lerpStartTime;

    public bool activated = false;
    public bool lerping { get; private set; }
    public bool aim { get; private set; }

    [SerializeField]
    private float x, y, z;

    void Start()
    {
        aim = false;
    }

    void LateUpdate()
    {
        //Refresh input
        if ((Input.GetAxis("L2") <= 0.2) && l2)
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
                transform.position = Vector3.Lerp(lerpStartPos, player.transform.position + player.transform.forward * -5 + Vector3.up * 3+player.transform.right*2, lerpPercent);
                center = player.transform.position + (targetEnemy.transform.position - player.transform.position) / 2;
                transform.LookAt(Vector3.Lerp(lerpFocus,center,lerpPercent));
            }
            else if (lookAt == "Aim")
            {
                var ab = transform.parent.rotation * init/2;
                var bc = player.transform.right * 1.5f + player.transform.up * 0.3f + player.transform.forward * -0.5f;
                transform.position = Vector3.Lerp(lerpStartPos, player.transform.position+ab+bc, lerpPercent);
                transform.LookAt(Vector3.Lerp(lerpFocus, player.transform.position+bc, lerpPercent));

            }
            else if (lookAt == "Player")
            {
                transform.position = Vector3.Lerp(lerpStartPos, player.transform.position + transform.parent.rotation * init, lerpPercent);
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
                    offset = transform.position - player.transform.position;
                }
                else
                {
                    offset = transform.parent.rotation * init/2;
                }
            }
        }
        else if (!activated)
        {
            return;
        }
        else {
            //targeting ennemy
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
                else if ((Input.GetAxis("L2") > 0.2 || Input.GetButton("L2")) && !l2)
                {
                    l2 = true;
                    setFollow();
                    return;
                }

                var mag = Mathf.Sqrt(Mathf.Pow(Mathf.Abs(targetEnemy.transform.position.x -transform.position.x),2) + Mathf.Pow(Mathf.Abs(targetEnemy.transform.position.z - transform.position.z),2));
                if (mag > 28f)
                {
                    setFollow();
                    return;
                }

                center = player.transform.position + (targetEnemy.transform.position - player.transform.position) / 2;
                transform.position = player.transform.position + player.transform.forward * -5 + Vector3.up * 3 + player.transform.right*2;
                lerpFocus = center;
                transform.LookAt(center);
            }
            //Free
            else
            {
                var cameraX = Input.GetAxis("RS-X") * 200* Time.deltaTime; ;
                var cameraHeight = new Vector3(0, Input.GetAxis("RS-Y") * 10f, 0) * Time.deltaTime; ;
                if (aim)
                {
                    cameraHeight /= 2;
                }

                var cameraDistance = -Input.GetAxis("RS-Y") * 10f * Time.deltaTime;

                if ((Input.GetAxis("L2") > 0.2 || Input.GetButton("L2")) && !l2)
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
                    return;
                }
                Quaternion rotation = Quaternion.Euler(0, cameraX, 0);
                transform.position = player.transform.position + (rotation * offset) + cameraHeight;
                if (transform.position.y > player.transform.position.y + maxHeight && !aim)
                {
                    transform.position = new Vector3(transform.position.x, player.transform.position.y + maxHeight, transform.position.z);
                }else if (transform.position.y > player.transform.position.y + 2.6f && aim)
                {
                    transform.position = new Vector3(transform.position.x, player.transform.position.y + 2.6f, transform.position.z);
                }

                if (transform.position.y < player.transform.position.y - 0.8f && !aim)
                {
                    transform.position = new Vector3(transform.position.x, player.transform.position.y - 0.8f, transform.position.z);
                }else if(transform.position.y < player.transform.position.y - 6f && aim)
                {
                    transform.position = new Vector3(transform.position.x, player.transform.position.y - 6f, transform.position.z);
                }

                    
                transform.LookAt(player.transform);
                transform.position += transform.forward * cameraDistance;
                var mag = Mathf.Abs(transform.localPosition.x) + Mathf.Abs(transform.localPosition.z);
                if (aim)
                {
                    if (mag < 1.2f)
                    {
                        transform.localPosition = new Vector3(transform.localPosition.x / mag * 1.2f, transform.localPosition.y, transform.localPosition.z / mag * 1.2f);
                    }
                    else if (mag > 2.4f)
                    {
                        transform.localPosition = new Vector3(transform.localPosition.x / mag * 2.4f, transform.localPosition.y, transform.localPosition.z / mag * 2.4f);
                    }
                }if (!aim)
                {
                    if (mag < 4f)
                    {
                        transform.localPosition = new Vector3(transform.localPosition.x / mag * 4, transform.localPosition.y, transform.localPosition.z / mag * 4);
                    }
                    else if (mag > 15f)
                    {
                        transform.localPosition = new Vector3(transform.localPosition.x / mag * 15, transform.localPosition.y, transform.localPosition.z / mag * 15);
                    }
                }

                offset = transform.position - player.transform.position;
                if (aim)
                {
                    transform.position += player.transform.right*1.5f + player.transform.up * 0.3f+ player.transform.forward *-0.5f;
                    lerpFocus = transform.position - offset;
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

    public void setAim(bool value)
    {
        aim = value;
        if (aim) {
            lerpStart(Vector3.zero, "Aim");
        }
        else
        {
            lerpStart(Vector3.zero, "Player");
        }
    }

    public void setFollow()
    {
        lerpStart(Vector3.zero, "Player");
        activated = true;
        enemyFocused = false;
    }

    public void setFocus(GameObject _target)
    {
        targetEnemy = _target;
        lerpStart(Vector3.zero, "Center");
        activated = true;
        enemyFocused = true;
    }


    public void setSpecific(GameObject _target, Vector3 offset)
    {
        target = _target.transform.position;
        lerpStart(target + offset, "Default");
        activated = false;
    }

    /*
    public void setSpecific(GameObject targetA, GameObject targetB, Vector3 offset)
    {
        Vector3 middle = (targetB.transform.position - targetA.transform.position) / 2;
        transform.position = middle + targetA.transform.right * 3;
        transform.LookAt(middle);
        locked = true;
    }*/

    public GameObject getFocus()
    {
        if (enemyFocused)
            return targetEnemy;
        else
            return null;
    }
    public void turnOn()
    {
        player = PlayerController.Instance.transform.GetChild(0).gameObject;
        transform.parent = player.transform;
        init = new Vector3(x, y, z);
        offset = init;
        setFollow();
    }

    public void turnOff()
    {
        transform.parent = GameController.Instance.transform;
        player = transform.parent.gameObject;
        activated = false;
    }

    public void lerpStart(Vector3 _target, string _lookAt)
    {
        lookAt = _lookAt;
        lerping = true;
        lerpStartTime = Time.time;
        lerpStartPos = transform.position;
        lerpTargetPos = _target;
    }

}