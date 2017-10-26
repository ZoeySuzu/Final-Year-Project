using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public GameObject target;
    public float maxHeight;
    public float maxDistance;
    private bool locked;

    //Vector3 defaultPosition;
    Vector3 offset;

    void Start()
    {
        locked = true;
        offset = target.transform.position - transform.position;
        //defaultPosition = new Vector3(0, 5, -8);
    }

    void LateUpdate()
    {
        var cameraX = -Input.GetAxis("Joystick_B X") * 2;
        var cameraHeight = new Vector3(0, Input.GetAxis("Joystick_B Y")*0.2f, 0);
        //var cameraDistance = -Input.GetAxis("Joystick_B Y") * 0.2f;

        if(locked && (cameraX != 0 || cameraHeight != Vector3.zero))
        {
            locked = false;
        }

        if (!locked && Input.GetAxis("CameraLock") == 1)
        {
            Debug.Log("camera lock");
            locked = true;
        }

        if (!locked)
        {
            Quaternion rotation = Quaternion.Euler(0, cameraX, 0);
            transform.position = target.transform.position - (rotation * offset) + cameraHeight;



            if (transform.position.y > target.transform.position.y + maxHeight)
            {
                transform.position = new Vector3(transform.position.x, target.transform.position.y + maxHeight, transform.position.z);
            }
            if (transform.position.y < target.transform.position.y)
            {
                transform.position = new Vector3(transform.position.x, target.transform.position.y, transform.position.z);
            }

            offset = target.transform.position - transform.position;
            transform.LookAt(target.transform);
        }
        else
        {
            transform.position = target.transform.position + target.transform.forward * -8 + target.transform.up * 5;
            offset = target.transform.position - transform.position;
            transform.LookAt(target.transform);
        }

    }
}