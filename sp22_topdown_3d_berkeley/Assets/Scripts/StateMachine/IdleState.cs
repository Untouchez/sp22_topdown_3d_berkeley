using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public ChaseState chase;
    public Vector3 startPos;
    public Vector3 startDir;

    public float seeDistance;

    public bool withinAngle;
    public bool withinSeeDistance;
    public bool canSeePlayer;

    public float distanceToPlayer;

    public void Start()
    {
        startPos = transform.position;
        startDir = me.transform.forward;
    }

    public override State RunCurrentState()
    {
        distanceToPlayer = Vector3.Distance(this.transform.position, player.position);
        withinSeeDistance = distanceToPlayer < seeDistance;
        if (Vector3.Distance(startPos, transform.position) >= agent.stoppingDistance + 0.5f)
        {
            agent.SetDestination(startPos);
            anim.SetFloat("speed", Mathf.Clamp(agent.velocity.magnitude / agent.speed,0,0.5f));
        }
        else
        {
            anim.SetFloat("speed", agent.velocity.magnitude / agent.speed);
            me.transform.forward = startDir;
        }

        if (!withinSeeDistance)
            return this;
        canSeePlayer = CanSeePlayer();

        if(canSeePlayer)
        {
            if(IsPlayerInfrontOfMe()) {
                chase.AwakeCurrentState();
                return chase;
            }
        }
        return this;
    }

    public override void AwakeCurrentState()
    {
        stateManager.updateRate = updateRate;
        print("entered: " + this);
    }
}
