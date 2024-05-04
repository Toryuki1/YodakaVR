using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class CalculateMatrix : MonoBehaviour 
{
    /// <summary>
    /// 目标位置的Transform组件
    /// </summary>
    public Transform dest;
    /// <summary>
    /// 当前父物体的Transform组件
    /// </summary>
    public Transform thisTransform;
    /// <summary>
    /// 子物体的Transform组件
    /// </summary>
    public Transform headTransform;
 

    void Awake()
    {
        SetTransform();
        InputEvent.Instance.onRightAXButtonDown += RightAXButtonDown;
        InputEvent.Instance.onRightAXButtonUp -= RightAXButtonDown;

    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SetTransform();
        }
    }
 
    void SetTransform()
    {
        // 计算目标位置与子物体的相对旋转，只保留Y轴旋转为0
        Vector3 relativeEulerAngles = dest.eulerAngles - headTransform.eulerAngles;
        relativeEulerAngles.x = 0f; // 保持Y轴旋转为0
        relativeEulerAngles.z = 0f; // 保持Y轴旋转为0
 
        // 根据相对旋转调整当前物体的欧拉角
        thisTransform.eulerAngles += relativeEulerAngles;
        
        // 根据相对位置调整当前物体的位置
        thisTransform.position += (dest.position - headTransform.position);
    }

        public void RightAXButtonDown()
        {
        SetTransform();
    }
}


