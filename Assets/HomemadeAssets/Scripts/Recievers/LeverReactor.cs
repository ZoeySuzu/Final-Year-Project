using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverReactor : MonoBehaviour {

    private Vector3 defaultPos;
    public void Awake()
    {
        defaultPos = transform.position;
    }

    public void updateReactor(float f, ReactionType r)
    {

        if (r == ReactionType.Horizontal) {
            transform.localPosition = new Vector3(f, transform.localPosition.y, transform.localPosition.z);
        }
        else if (r == ReactionType.Vertical)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, f, transform.localPosition.z);
        }

    }
	
}
