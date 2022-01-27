using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    public ChaseState chaseState;
    public float attackRange;
    public bool canUpdate;

    public override State RunCurrentState()
    {
        if (Vector3.Distance(this.transform.position, player.position) > attackRange+2 || !CanSeePlayer())
        {
            canUpdate = false;
            return chaseState;
        }
        agent.SetDestination(transform.position);
        canUpdate = true;
        return this;
    }

    public void Update()
    {
        if (!canUpdate)
            return;
        anim.SetFloat("speed", Mathf.MoveTowards(anim.GetFloat("speed"), 0, 1f*Time.deltaTime));
        me.transform.LookAt(player);
    }

    public override void AwakeCurrentState()
    {
        stateManager.updateRate = updateRate;
        print("entered: " + this);
    }
}
