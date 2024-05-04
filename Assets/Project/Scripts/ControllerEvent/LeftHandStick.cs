using UnityEngine;

public class LeftHandStick : MonoBehaviour
{
    private void Awake()
    {
        InputEvent.Instance.onLeftJoyStickMove += LeftJoyStickMove;

    }
    void LeftJoyStickMove(Vector2 joystickValue)
    {
        print("移动摇杆");
    }

}

