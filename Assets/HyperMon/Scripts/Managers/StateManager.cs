using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager 
{
    private Dictionary<int, IState> m_States;
    public IState CurrentState { get; private set; }
    public IState PreviousState { get; private set; }

    private bool m_InTransition;

    public StateManager()
    {
        m_InTransition = false;
        CurrentState = null;
        PreviousState = null;
        m_States = new Dictionary<int, IState>();
    }

    public void StartState(State.StateType state)
    {
        PreviousState = null;
        CurrentState = m_States[0];
        CurrentState.Enter();
    }

    public void AddState(IState state)
    {
        m_States.Add(state.ID, state);
    }

    public void RevertState()
    {
        if (PreviousState != null)
            ChangeState((State.StateType)PreviousState.ID);
    }

    public void ChangeState(State.StateType state)
    {
        int stateID = (int)state;

        if (CurrentState.ID == stateID || m_InTransition)
            return;

        if(m_States.ContainsKey(stateID))
        {
            m_InTransition = true;
            IState newState = m_States[stateID];

            if (CurrentState != null)
                CurrentState.Exit();

            PreviousState = CurrentState;
            CurrentState = newState;
            CurrentState.Enter();
        }
        m_InTransition = false;
    }
    public void UpdateController()
    {
        if (CurrentState != null && !m_InTransition)
        {
            CurrentState.Update();
        }
    }
}
