using UnityEngine;
using UnityEngine.InputSystem;



public class MovementSM : PlayerStateMachine
{
    [HideInInspector]
    public IdleState idleState;

    [HideInInspector]
    public WalkingState walkingState;

    [HideInInspector]
    public GlidingState glidingState;

    [HideInInspector]
    public FlappingState flappingState;

    public Transform player;
    public Transform leftController;
    public Transform rightController;

    public void Awake()
    {

        idleState = new IdleState(this);
        walkingState = new WalkingState(this);
        glidingState = new GlidingState(this);
        flappingState = new FlappingState(this);


    }

    protected override PlayerState GetInitialState()
    {
        return idleState;
    }

    public PlayerState GetCurrentState()
    {
        return currentPlayerState;
    }
}


