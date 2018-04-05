using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour {

    bool chainAttack;
    bool hit;
    int attackN;
    float lerpStartTime;
    Vector3 lerpStartPos, lerpTargetPos;

	// Use this for initialization
	void Start () {
        hit = false;
        attackN = 0;
        firstAttack();
        chainAttack = false;
        transform.parent = PlayerController.Instance.getWand().transform;
        transform.position = transform.parent.position + transform.parent.up*1f;
        transform.rotation = transform.parent.rotation;
	}

    private void firstAttack()
    {
        PlayerController.Instance.anim.SetTrigger("Attack1");
    }

    private void secondAttack()
    {
        hit = false;
        attackN++;
        chainAttack = false;
        PlayerController.Instance.anim.SetTrigger("Attack2");
    }


    private void thirdAttack()
    {
        hit = false;
        attackN++;
        chainAttack = false;
    }

    // Update is called once per frame
    void Update () {
        if (attackN < 2 && Input.GetButtonDown("X"))
        {
            chainAttack = true;
        }

        if (attackN == 0 && PlayerController.Instance.anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.6f && chainAttack)
        {
            secondAttack();
        }
        else if (PlayerController.Instance.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >=0.7f)
        {
            PlayerController.Instance.refreshAttack();
            Destroy(gameObject);
        }
    }


    public void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy" && !hit)
        {
            hit = true;
            Vector3 dir = PlayerController.Instance.transform.GetChild(0).forward;
            EnemyController ec = other.GetComponentInParent<EnemyController>();
            if (attackN < 2)
            {
                ec.knockback(dir*1.5f+Vector3.up*1.2f);
                ec.setHealth(-3);
            }
            else
            {
                ec.knockback(dir*3+Vector3.up*1.5f);
                ec.setHealth(-5);
            }
        }
    }

}
