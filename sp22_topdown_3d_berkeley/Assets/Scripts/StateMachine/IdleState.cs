using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public LayerMask ignore;
    public ChaseState chase;
    public bool canSeePlayer;
    public float seeDistance;

    public override State RunCurrentState()
    {
        
        canSeePlayer = Vector3.Distance(this.transform.position,player.position)<seeDistance;
        if(canSeePlayer && CanSeePlayer())
        {
            return chase;
        }
        anim.SetFloat("horizontal", Mathf.SmoothStep(anim.GetFloat("horizontal"), 0, RandomTransitionSpeed()));
        anim.SetFloat("vertical", Mathf.SmoothStep(anim.GetFloat("vertical"), 0, RandomTransitionSpeed()));
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

    public float RandomTransitionSpeed()
    {
        return Random.Range(0.3f, 0.6f);
    }
}
