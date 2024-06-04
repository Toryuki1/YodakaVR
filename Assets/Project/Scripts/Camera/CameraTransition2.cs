using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraTransition2 : MonoBehaviour
{
   public List<GameObject> objectsToEnable; // 存储需要启用的物体
    public List<GameObject> objectsToDisable; // 存储需要关闭的物体

    // 此方法将在Timeline播放完成时被调用
    public void OnTimelineFinished()
    {
        // 启动协程，先触发眨眼效果，再延迟执行启用/禁用操作
        StartCoroutine(BlinkAndTransition());
    }

    private IEnumerator BlinkAndTransition()
    {
        // 触发眨眼效果
        BlinkEffect.Instance.TriggerBlink();
        
        // 等待 0.5 秒
        yield return new WaitForSeconds(0.5f);

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