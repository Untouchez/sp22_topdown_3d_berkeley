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


    public abstract State RunCurrentState();


    public abstract void AwakeCurrentState();



}
