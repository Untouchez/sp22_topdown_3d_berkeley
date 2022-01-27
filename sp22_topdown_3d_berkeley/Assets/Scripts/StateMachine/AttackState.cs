using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    public LayerMask ignore;
    public ChaseState chaseState;
    public float attackRange;
    public bool canUpdate;
    public float updateRate;

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

    public bool CanSeePlayer()
    {
        Vector3 dir = (player.position - transform.position).normalized * attackRange;
        if(Physics.SphereCast(transform.position,0.5f,dir,out RaycastHit hit,attackRange,~ignore))
        {
            if(hit.transform.root.name != "Player")
            {
                return false;
            }
            return true;
        }
        return false;
    }

    public void Update()
    {
        if (!canUpdate)
            return;
        anim.SetFloat("speed", Mathf.MoveTowards(anim.GetFloat("speed"), 0, 1f*Time.deltaTime));
    }

    public override void AwakeCurrentState()
    {
        stateManager.updateRate = updateRate;
        print("entered: " + this);
    }
}
