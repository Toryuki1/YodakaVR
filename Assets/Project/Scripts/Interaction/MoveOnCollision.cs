using UnityEngine;
using System.Collections.Generic;

public class MoveOnCollision : MonoBehaviour
{
    public List<Transform> positions; // 位置列表
    public float yOffset = 1f; // Y轴偏移量
    public float movementSpeed = 1f; // 移动速度

    private bool shouldMove = false;
    private int currentPositionIndex = 0;
    private float startTime;
    private float journeyLength;

    void Update()
    {
        if (shouldMove)
        {
            // 计算当前位置和目标位置之间的距离比例
            float distCovered = (Time.time - startTime) * movementSpeed;
            float fracJourney = distCovered / journeyLength;

            // 在两点之间插值移动，将Y轴偏移量应用到目标位置
            Vector3 targetPosition = positions[currentPositionIndex + 1].position;
            targetPosition.y += yOffset;
            transform.position = Vector3.Lerp(positions[currentPositionIndex].position, targetPosition, fracJourney);

            // 如果已经到达目标位置，停止移动并更新目标位置索引
            if (fracJourney >= 1f)
            {
                shouldMove = false;
                currentPositionIndex++;

                // 检查是否到达位置列表的末尾，如果是，则将当前位置索引重置为0
                if (currentPositionIndex >= positions.Count - 1)
                {
                    currentPositionIndex = 0;
                }
            }
        }
    }

    // 使用 OnTriggerEnter 方法来检测碰撞
    void OnTriggerEnter(Collider other)
    {
        // 检查碰撞是否是与玩家的碰撞
        if (other.gameObject.CompareTag("Player"))
        {
            StartMoving();
        }
    }

    void StartMoving()
    {
        startTime = Time.time;
        Vector3 targetPosition = positions[currentPositionIndex + 1].position;
        targetPosition.y += yOffset;
        journeyLength = Vector3.Distance(positions[currentPositionIndex].position, targetPosition);
        shouldMove = true;
    }
}
