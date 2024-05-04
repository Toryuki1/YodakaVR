using UnityEngine;

public class WalkingState : PlayerState
{
    private MovementSM _sm;

    public WalkingState(MovementSM stateMachine) : base("WalkingState", stateMachine)
    {
        _sm = stateMachine;
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

    }
}
