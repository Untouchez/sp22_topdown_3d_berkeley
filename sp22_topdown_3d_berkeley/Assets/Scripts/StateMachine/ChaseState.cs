using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    public AttackState attackState;
    public AlarmedState alarmedState;
    public IdleState idleState;
    public float acceleration;
    public bool canUpdate;
    public bool canSeePlayer;
    float distance;

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
                alarmedState.AwakeCurrentState();
                return alarmedState;
            }
        } else {
            transform.LookAt(player);
            //IF WITHIN ATTACK RANGE AND CAN SEE PLAYER THEN ATTACK
            if (distance < attackState.attackRange) {
                attackState.AwakeCurrentState();
                return attackState;
            }

            //RETURNS TO IDLE IF OUTSIDE OF SEE DISTANCE
            if (distance > idleState.seeDistance * 2f) {
                idleState.AwakeCurrentState();
                return idleState;
            }
            lastSeenPlayer = player.position + (player.forward*0.5F);
        }
        agent.SetDestination(lastSeenPlayer);
        return this;
    }

    public override void AwakeCurrentState()
    {
        stateManager.updateRate = updateRate;
        lastSeenPlayer = player.transform.position;
        print("entered: " + this);
    }
}
