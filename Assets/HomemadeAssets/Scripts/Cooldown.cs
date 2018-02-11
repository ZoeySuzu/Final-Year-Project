using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooldown  {

    public bool ready;
    private float cooldownTime;

    public Cooldown(float duration)
    {
        ready = true ;
        cooldownTime = duration;
    }

    public void setCooldownTime(float duration)
    {
        cooldownTime = duration;
    }


    public IEnumerator StartCooldown()
    {
        ready = false;
        yield return new WaitForSeconds(cooldownTime);
        ready = true;
    }


}
