using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
/// <summary>
/// 提供各种输入事件
/// </summary>
public class InputEvent : Common.MonoSingleton<InputEvent>
{
    //*************输入设别**************************
    InputDevice leftHandController;
    InputDevice rightHandController;
    InputDevice headController;

    //**************对外提供公开事件******************
    #region public event

    public Action onLeftTriggerEnter;
    public Action onLeftTriggerDown;
    public Action onLeftTriggerUp;

    public Action onRightTriggerEnter;
    public Action onRightTriggerDown;
    public Action onRightTriggerUp;

    public Action onLeftGripEnter;
    public Action onLeftGripDown;
    public Action onLeftGripUp;

    public Action onRightGripEnter;
    public Action onRightGripDown;
    public Action onRightGripUp;

    public Action onLeftAppButtonEnter;
    public Action onLeftAppButtonDown;
    public Action onLeftAppButtonUp;

    public Action onRightAppButtonEnter;
    public Action onRightAppButtonDown;
    public Action onRightAppButtonUp;

    public Action onLeftJoyStickEnter;
    public Action onLeftJoyStickDown;
    public Action onLeftJoyStickUp;

    public Action onRightJoyStickEnter;
    public Action onRightJoyStickDown;
    public Action onRightJoyStickUp;

    public Action<Vector2> onLeftJoyStickMove;
    public Action<Vector2> onRightJoyStickMove;

    public Action onLeftAXButtonEnter;
    public Action onLeftAXButtonDown;
    public Action onLeftAXButtonUp;

    public Action onLeftBYButtonEnter;
    public Action onLeftBYButtonDown;
    public Action onLeftBYButonUp;

    public Action onRightAXButtonEnter;
    public Action onRightAXButtonDown;
    public Action onRightAXButtonUp;

    public Action onRightBYButtonEnter;
    public Action onRightBYButtonDown;
    public Action onRightBYButtonUp;

    #endregion

    //提供状态字典独立记录各个feature的状态
    Dictionary<string, bool> stateDic;

    private void Init()
    {
        if (!leftHandController.isValid)
        {
            leftHandController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        }
        if (!rightHandController.isValid)
        {
            rightHandController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        }
        if (!headController.isValid)
        {
            headController = InputDevices.GetDeviceAtXRNode(XRNode.Head);
        }
        stateDic = new Dictionary<string, bool>();

    }

    //*******************事件源的触发**************************
    /// <summary>
    /// 按钮事件源触发模板
    /// </summary>
    /// <param name="device">设备</param>
    /// <param name="usage">功能特征</param>
    /// <param name="btnEnter">开始按下按钮事件</param>
    /// <param name="btnDown">按下按钮事件</param>
    /// <param name="btnUp">抬起按钮事件</param>
    private void ButtonDispatchModel(InputDevice device, InputFeatureUsage<bool> usage, Action btnEnter, Action btnDown, Action btnUp)
    {
        //Debug.Log("usage:" + usage.name);
        //为首次执行的feature添加bool状态 -- 用以判断Enter和Up状态
        string featureKey = device.characteristics + usage.name;
        if (!stateDic.ContainsKey(featureKey))
        {
            stateDic.Add(featureKey, false);
        }

        bool isDown;
        if (device.TryGetFeatureValue(usage, out isDown) && isDown)
        {
            //Debug.Log("device:" + device.characteristics + "usage:" + usage.name);
            if (!stateDic[featureKey])
            {
                stateDic[featureKey] = true;
                btnEnter?.Invoke();
            }
            btnDown?.Invoke();
        }
        else
        {
            if (stateDic[featureKey])
            {
                btnUp?.Invoke();
                stateDic[featureKey] = false;
            }
        }
    }

    /// <summary>
    /// 摇杆事件源触发模板
    /// </summary>
    /// <param name="device">设备</param>
    /// <param name="usage">功能特征</param>
    /// <param name="joyStickMove">移动摇杆事件</param>
    private void JoyStickDispatchModel(InputDevice device, InputFeatureUsage<Vector2> usage, Action<Vector2> joyStickMove)
    {
        Vector2 axis;
        if (device.TryGetFeatureValue(usage, out axis) && !axis.Equals(Vector2.zero))
        {
            if (joyStickMove != null)
                joyStickMove(axis);
        }
    }

    //******************每帧轮询监听事件***********************
    private void Update()
    {
        if (leftHandController.isValid)
        {
            ButtonDispatchModel(leftHandController, CommonUsages.triggerButton, onLeftTriggerEnter, onLeftTriggerDown, onLeftTriggerUp);
            ButtonDispatchModel(leftHandController, CommonUsages.gripButton, onLeftGripEnter, onLeftGripDown, onLeftGripUp);
            ButtonDispatchModel(leftHandController, CommonUsages.primaryButton, onLeftAXButtonEnter, onLeftAXButtonDown, onLeftAXButtonUp);
            ButtonDispatchModel(leftHandController, CommonUsages.secondaryButton, onLeftBYButtonEnter, onLeftBYButtonDown, onLeftBYButonUp);
            ButtonDispatchModel(leftHandController, CommonUsages.primary2DAxisClick, onLeftJoyStickEnter, onLeftJoyStickDown, onLeftJoyStickUp);
            ButtonDispatchModel(leftHandController, CommonUsages.menuButton, onLeftAppButtonEnter, onLeftAppButtonDown, onLeftAppButtonUp);
            JoyStickDispatchModel(leftHandController, CommonUsages.primary2DAxis, onLeftJoyStickMove);
        }
        else
        {
            Init();
        }
        if (rightHandController.isValid)
        {
            ButtonDispatchModel(rightHandController, CommonUsages.triggerButton, onRightTriggerEnter, onRightTriggerDown, onRightTriggerUp);
            ButtonDispatchModel(rightHandController, CommonUsages.gripButton, onRightGripEnter, onRightGripDown, onRightGripUp);
            ButtonDispatchModel(rightHandController, CommonUsages.primaryButton, onRightAXButtonEnter, onRightAXButtonDown, onRightAXButtonUp);
            ButtonDispatchModel(rightHandController, CommonUsages.secondaryButton, onRightBYButtonEnter, onRightBYButtonDown, onRightBYButtonUp);
            ButtonDispatchModel(rightHandController, CommonUsages.primary2DAxisClick, onRightJoyStickEnter, onRightJoyStickDown, onRightJoyStickUp);
            ButtonDispatchModel(rightHandController, CommonUsages.menuButton, onRightAppButtonEnter, onRightAppButtonDown, onRightAppButtonUp);
            JoyStickDispatchModel(rightHandController, CommonUsages.primary2DAxis, onRightJoyStickMove);
        }
        else
        {
            Init();
        }
    }
}
