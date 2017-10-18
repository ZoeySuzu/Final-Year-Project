using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public GameObject target;
    public float maxHeight;
    public float maxDistance;
    Vector3 offset;

    void Start()
    {
        offset = target.transform.position - transform.position;
    }

    void LateUpdate()
    {
        var cameraX = -Input.GetAxis("Joystick_B X") * 2;
        var cameraHeight = new Vector3(0, Input.GetAxis("Joystick_B Y")*0.2f, 0);
        //var cameraDistance = -Input.GetAxis("Joystick_B Y") * 0.2f;

        Quaternion rotation = Quaternion.Euler(0, cameraX, 0);
        transform.position = target.transform.position - (rotation * offset)+cameraHeight;





        if(transform.position.y > target.transform.position.y + maxHeight)
        {
            transform.position = new Vector3(transform.position.x, target.transform.position.y+maxHeight, transform.position.z);
        }
        if (transform.position.y < target.transform.position.y)
        {
            transform.position = new Vector3(transform.position.x, target.transform.position.y, transform.position.z);
        }
        
        offset = target.transform.position - transform.position;

        transform.LookAt(target.transform);





    }
}