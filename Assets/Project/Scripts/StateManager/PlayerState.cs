using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlayerState
{
    protected readonly MovementSM playerStateMachine;

    public string name { get; private set; }

    public PlayerState(string stateName, MovementSM stateMachine)
    {
        name = stateName;
        playerStateMachine = stateMachine;
    }

    public virtual void EnterState()
    {
    }

    public virtual void ExitState()
    {
    }

    public virtual void UpdateLogic()
    {
        // Implement logic for this state
    }

    public virtual void UpdatePhysics()
    {
        // Implement physics update for this state
    }
}
