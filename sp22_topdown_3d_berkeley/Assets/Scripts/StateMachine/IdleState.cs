using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public LayerMask ignore;
    public ChaseState chase;

    public float nearRadius;
    public float angleRadius;
    public float seeDistance;
    public float updateRate;

    public bool withinAngle;
    public bool withinSeeDistance;
    public bool canSeePlayer;

    public float distanceToPlayer;
    public float angle;


    public override State RunCurrentState()
    {
        anim.SetFloat("speed", agent.velocity.magnitude / agent.speed);
        distanceToPlayer = Vector3.Distance(this.transform.position, player.position);
        withinSeeDistance = distanceToPlayer < seeDistance;

        if (!withinSeeDistance)
            return this;
        canSeePlayer = CanSeePlayer();

        if(canSeePlayer)
        {
            //CHECK IF CLOSE TO PLAYER 
            if (distanceToPlayer < nearRadius)
            {
                chase.AwakeCurrentState();
                return chase;
            }
            angle = Vector3.Angle(transform.forward, player.position - transform.position);

            //CHECK IF PLAYER IS INFRONT OF ME
            withinAngle = angle < angleRadius;
            if (withinAngle)
            {
                chase.AwakeCurrentState();
                return chase;
            }
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

    public override void AwakeCurrentState()
    {
        stateManager.updateRate = updateRate;
        print("entered: " + this);
    }
}
