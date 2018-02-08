using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraID : MonoBehaviour {
    [SerializeField]
    private string id;

    [SerializeField]
    private Vector3 vec;

    void Start()
    {
        GameController.Instance.addCameraId(this);
    }

    public string getId()
    {
        return id;
    }

    public Vector3 getVec()
    {
        return vec;
    }
}
