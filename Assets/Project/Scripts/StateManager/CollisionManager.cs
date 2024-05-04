using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    private MovementSM movementSM;

    private void Start()
    {
        // 获取角色状态机
        movementSM = FindObjectOfType<MovementSM>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 如果角色状态机存在，并且当前状态是 FlappingState 或 GlidingState，则传递碰撞信息给它的当前状态
        if (movementSM != null && (movementSM.currentPlayerState is FlappingState || movementSM.currentPlayerState is GlidingState))
        {
            if (movementSM.currentPlayerState is FlappingState)
            {
                (movementSM.currentPlayerState as FlappingState).HandleCollisionEnter(collision);
            }
            else if (movementSM.currentPlayerState is GlidingState)
            {
                (movementSM.currentPlayerState as GlidingState).HandleCollisionEnter(collision);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // 如果角色状态机存在，并且当前状态是 FlappingState 或 GlidingState，则传递碰撞信息给它的当前状态
        if (movementSM != null && (movementSM.currentPlayerState is FlappingState || movementSM.currentPlayerState is GlidingState))
        {
            if (movementSM.currentPlayerState is FlappingState)
            {
                (movementSM.currentPlayerState as FlappingState).HandleCollisionExit(collision);
            }
            else if (movementSM.currentPlayerState is GlidingState)
            {
                (movementSM.currentPlayerState as GlidingState).HandleCollisionExit(collision);
            }
        }
    }
}
