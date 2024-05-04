using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float maxTurnSpeed = 10f; // 最大转向速度
    public float turnAcceleration = 5f; // 转向加速度
    public Transform point1; // 第一个点位物体
    public Transform point2; // 第二个点位物体
    public Transform playerTransform; // 玩家物体的Transform组件
    public float deadZoneAngle = 1f; // 死区角度

    private float currentTurnSpeed = 0f; // 当前转向速度

    // 函数用于让玩家物体朝向目标方向
    public void TurnTowardsDirection(Vector3 targetDirection)
    {
        // 计算当前朝向与目标方向之间的角度差
        float angleDifference = Vector3.Angle(playerTransform.forward, targetDirection);

        // 如果角度差小于设定的死区阈值，则不进行转向
        if (angleDifference < deadZoneAngle)
        {
            return;
        }

        // 根据当前转向速度计算每帧应该旋转的角度
        float step = Mathf.Min(currentTurnSpeed * Time.deltaTime, angleDifference);

        // 使用Vector3.RotateTowards函数逐步旋转玩家物体朝向目标方向
        Vector3 newDirection = Vector3.RotateTowards(playerTransform.forward, targetDirection, step, 0f);

        // 更新玩家物体的朝向
        playerTransform.rotation = Quaternion.LookRotation(newDirection);

        // 根据转向加速度更新当前转向速度
        currentTurnSpeed = Mathf.MoveTowards(currentTurnSpeed, maxTurnSpeed, turnAcceleration * Time.deltaTime);
    }

    // 在Update函数中调用TurnTowardsDirection来让玩家朝向两个点位之间的连线的垂直单位向量
    void Update()
    {
        // 获取两个点位之间的连线的垂直单位向量
        Vector3 perpendicularVector = CalculateVerticalUnitVector(point1.position, point2.position);

        // 调用TurnTowardsDirection函数，让玩家朝向垂直单位向量
        TurnTowardsDirection(perpendicularVector);
    }

    // 函数用于计算两点间的水平面上的垂直单位向量
    private Vector3 CalculateVerticalUnitVector(Vector3 point1, Vector3 point2)
    {
        // 将两个点投影到水平面
        Vector3 projectedPoint1 = new Vector3(point1.x, 0f, point1.z);
        Vector3 projectedPoint2 = new Vector3(point2.x, 0f, point2.z);

        // 计算连线向量（从 point1 指向 point2）
        Vector3 lineVector = projectedPoint2 - projectedPoint1;

        // 如果线的长度为零，则返回零向量
        if (lineVector.magnitude == 0f)
        {
            Debug.LogWarning("Points are coincident, cannot calculate perpendicular vector.");
            return Vector3.zero;
        }

        // 计算水平面上的垂直单位向量
        Vector3 perpendicularVector = new Vector3(lineVector.z, 0f, -lineVector.x).normalized;

        return perpendicularVector;
    }
}
