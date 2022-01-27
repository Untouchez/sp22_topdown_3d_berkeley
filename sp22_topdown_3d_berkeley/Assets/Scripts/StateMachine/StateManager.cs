using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateManager : MonoBehaviour
{
    public Transform Me;
    public Transform player;
    public State currentState;

    public float updateRate;
    private float timeRemaining;

    public void Awake()
    {
        foreach(State state in GetComponentsInChildren<State>())
        {
            state.me = Me;
            state.anim = Me.GetComponent<Animator>();
            state.agent = Me.GetComponent<NavMeshAgent>();

            state.player = player;
            state.stateManager = this;
        }
        timeRemaining = updateRate;
    }

    private void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }else
        {
            timeRemaining = updateRate;
            RunStateMachine();
        }
    }

    private void RunStateMachine()
    {
        State nextState = currentState?.RunCurrentState();
        if (nextState != null)
            SwitchToTheNextStateMachine(nextState);
    }

    public void SwitchToTheNextStateMachine(State nextState)
    {
        currentState = nextState;
    }
}
