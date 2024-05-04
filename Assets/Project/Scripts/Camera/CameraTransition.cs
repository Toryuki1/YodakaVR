using UnityEngine;
using System.Collections.Generic;

public class CameraTransition : MonoBehaviour
{
    public List<GameObject> objectsToEnable; // 存储需要启用的物体
    public List<GameObject> objectsToDisable; // 存储需要关闭的物体

    // 此方法将在Timeline播放完成时被调用
    public void OnTimelineFinished()
    {
        // 遍历并关闭所有需要关闭的物体
        foreach (GameObject obj in objectsToDisable)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }

        // 遍历并启用所有需要启用的物体
        foreach (GameObject obj in objectsToEnable)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
    }
}
