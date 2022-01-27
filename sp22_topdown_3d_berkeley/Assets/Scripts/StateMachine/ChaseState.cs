using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    public LayerMask ignore;
    public AttackState attackState;
    public IdleState idleState;
    public float acceleration;
    public bool canUpdate;
    public bool canSeePlayer;
    public float updateRate;
    public float distance;

    public Vector3 lastSeenPlayer;

    public override State RunCurrentState()
    {
        distance = Vector3.Distance(this.transform.position, player.position);
        canUpdate = true;
        canSeePlayer = CanSeePlayer();

        anim.SetFloat("speed", agent.velocity.magnitude/agent.speed);
        //IF CANT SEE PLAYER
        if (canSeePlayer == false)
        {
            transform.LookAt(lastSeenPlayer);
            //HAS COMPLETED MOVEMENT
            if(Vector3.Distance(lastSeenPlayer,transform.position) <=agent.stoppingDistance+0.2f) {
                idleState.AwakeCurrentState();
                return idleState;
            }
        }
        else
        {
            //IF WITHIN ATTACK RANGE AND CAN SEE PLAYER THEN ATTACK
            if (distance < attackState.attackRange)
                return attackState;

            //RETURNS TO IDLE IF OUTSIDE OF SEE DISTANCE
            if (distance > idleState.seeDistance * 2f)
            {
                idleState.AwakeCurrentState();
                return idleState;
            }
            lastSeenPlayer = player.position + (player.forward*0.5F);
        }
        agent.SetDestination(lastSeenPlayer);
        return this;
    }

    public bool CanSeePlayer()
    {
        Vector3 dir = (player.position - transform.position).normalized;
        if (Physics.SphereCast(transform.position, 0.1f, dir, out RaycastHit hit, 100f, ~ignore))
        {
            if (hit.transform.root.name != "Player")            
                return false;
            return true;
        }
        return false;
    }

    public override void AwakeCurrentState()
    {
        stateManager.updateRate = updateRate;
        lastSeenPlayer = player.transform.position;
        print("entered: " + this);
    }
}
