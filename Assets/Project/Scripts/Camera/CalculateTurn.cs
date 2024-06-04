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

    void Start()
    {
        StartCoroutine(DelayedSetTransform(0.1f));  // 启动延迟校正协程
        InputEvent.Instance.onRightAXButtonDown += RightAXButtonDown;
        InputEvent.Instance.onRightAXButtonUp -= RightAXButtonDown;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetTransform();
        }
    }

    private IEnumerator DelayedSetTransform(float delay)
    {
        yield return new WaitForSeconds(delay);  // 等待指定的秒数
        SetTransform();
    }

    void SetTransform()
    {
        // 计算目标位置与子物体的相对旋转，只保留Y轴旋转为0
        Vector3 relativeEulerAngles = dest.eulerAngles - headTransform.eulerAngles;
        relativeEulerAngles.x = 0f; // 保持X轴旋转为0
        relativeEulerAngles.z = 0f; // 保持Z轴旋转为0

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
