using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AlarmedState : State
{
    public AttackState attackState;
    public IdleState idleState;
    public ChaseState chaseState;
    float distance;

    public override void AwakeCurrentState()
    {
        Search();
    }

    public override State RunCurrentState()
    {
        distance = Vector3.Distance(this.transform.position, player.position);
        if(CanSeePlayer() && distance < idleState.seeDistance) {
            chaseState.AwakeCurrentState();
            return chaseState;
        }
        if(agent.velocity.magnitude <=1)
        {
            idleState.AwakeCurrentState();
            return idleState;
        }
        return this;
    }

    public void Search()
    {   
        agent.SetDestination(GetPlayerPos(distance));
    }

    //https://answers.unity.com/questions/475066/how-to-get-a-random-point-on-navmesh.html
    public Vector3 GetPlayerPos(float radius)
    {
        Vector3 randomDirection = (player.position - transform.position).normalized * idleState.seeDistance;
        //Vector3 randomDirection = player.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }
}
