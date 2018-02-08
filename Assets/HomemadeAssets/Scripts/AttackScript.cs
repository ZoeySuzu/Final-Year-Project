using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour {

    bool chainAttack;
    int attackN;
    float lerpStartTime;
    Vector3 lerpStartPos, lerpTargetPos;

	// Use this for initialization
	void Start () {
        attackN = 0;
        firstAttack();
        chainAttack = false;
	}

    private void firstAttack()
    {
        lerpStartTime = Time.time;
        lerpStartPos = transform.position;
        lerpTargetPos = transform.position+ transform.up * -1 + transform.right * -2;
    }

    private void secondAttack()
    {
        attackN++;
        chainAttack = false;
        lerpStartTime = Time.time;
        transform.localPosition = new Vector3(-1, 0f, 1.5f);
        lerpStartPos = transform.position;
        lerpTargetPos = transform.position + transform.up * 1.5f + transform.right * 1.5f;
    }


    private void thirdAttack()
    {
        attackN++;
        chainAttack = false;
        lerpStartTime = Time.time;
        transform.localPosition = new Vector3(0.5f, 1.5f, 1.5f);
        lerpStartPos = transform.position;
        lerpTargetPos = transform.position + transform.up * -2f + transform.right *-0.5f;
    }

    // Update is called once per frame
    void Update () {
        if(attackN<2 && Input.GetButtonDown("X"))
            {
                chainAttack = true;
            }

        float lerpTime = Time.time - lerpStartTime;
        float lerpPercent = lerpTime / 0.2f;
        if (lerpPercent < 1f)
        {
            transform.position = Vector3.Lerp(lerpStartPos, lerpTargetPos, lerpPercent);
        }
        else if (lerpPercent >= 1f)
        {
            if (chainAttack && attackN == 0)
            {
                secondAttack();
            }
            else if (chainAttack && attackN == 1)
            {
                thirdAttack();
            }
        }
        if (lerpPercent >=2f)
        {
            PlayerController.Instance.refreshAttack();
            Destroy(gameObject);
        }
    }
}
