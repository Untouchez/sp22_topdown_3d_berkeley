using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class State : MonoBehaviour
{
    [HideInInspector]
    public Transform player;
    [HideInInspector]
    public Transform me;
    [HideInInspector]
    public Animator anim;
    [HideInInspector]
    public Rigidbody rigid;
    [HideInInspector]
    public NavMeshAgent agent;
    [HideInInspector]
    public StateManager stateManager;

    float currentAngle;
    public float angleRadius;
    public LayerMask ignore;
    public float updateRate;

    public abstract State RunCurrentState();

    public abstract void AwakeCurrentState();

    public bool IsPlayerInfrontOfMe()
    {
        currentAngle = Vector3.Angle(transform.forward, player.position - transform.position);
        //CHECK IF PLAYER IS INFRONT OF ME
        if (currentAngle < angleRadius)
        {
            return true;
        }
        return false;
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

}
