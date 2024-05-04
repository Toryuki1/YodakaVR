using UnityEngine;

public class LeftHandTrigger : MonoBehaviour
{
    private void Awake()
    {
        InputEvent.Instance.onLeftTriggerUp += LeftTriggerUp;
        InputEvent.Instance.onLeftTriggerEnter += LeftTriggerEnter;
        InputEvent.Instance.onLeftTriggerDown += LeftTriggerDown;
    }
    void LeftTriggerDown()
    {
        print("左手柄A键按下中···");
    }
    void LeftTriggerEnter()
    {
        print("按下左手柄A键");
    }
    void LeftTriggerUp()
    {
        print("抬起左手柄A键");
    }
}

