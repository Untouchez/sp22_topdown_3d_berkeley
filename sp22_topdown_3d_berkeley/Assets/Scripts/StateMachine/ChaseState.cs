using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    public LayerMask ignore;
    public AttackState attackState;
    public float acceleration;
    public bool canUpdate;

    
    public override State RunCurrentState()
    {
        canUpdate = true;
        agent.SetDestination(player.position);
        anim.SetFloat("speed", agent.velocity.magnitude/agent.speed);
        if(Vector3.Distance(this.transform.position, player.position) < attackState.attackRange && CanSeePlayer()){
            canUpdate = false;
            return attackState;
        }
        return this;
    }
    public bool CanSeePlayer()
    {
        Vector3 dir = (player.position - transform.position).normalized;
        if (Physics.SphereCast(transform.position, 0.5f, dir, out RaycastHit hit, 100f, ~ignore))
        {
            if (hit.transform.root.name != "Player")
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
    }
}
