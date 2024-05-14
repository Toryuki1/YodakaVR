using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GlidingState : PlayerState
{
    private MovementSM _sm;
    private float glideForce = -9f; // 自定义的滑翔力大小

    private Transform leftController;
    private Transform rightController;

    private Vector3 glideDeriction;
    private float glideFoward = 0.2f;

    public float threshold = 0.1f; // 差异阈值
    public float forceMagnitude = 1f; // 施加的力大小

    public float rotationSpeed = 8f; // 旋转速度（每秒度）
    public float maxSpeed = 8.5f;

    public GlidingState(MovementSM stateMachine) : base("GlidingState", stateMachine)
    {
        _sm = stateMachine;
    }

    public override void EnterState()
    {
        base.EnterState();
        _sm.player.GetComponent<ActionBasedContinuousTurnProvider>().enabled = false;
        _sm.player.GetComponent<ActionBasedContinuousMoveProvider>().enabled = false;
        leftController = _sm.leftController;
        rightController = _sm.rightController;
        Rigidbody rigidbody = _sm.player.GetComponent<Rigidbody>();
        rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0.2f, rigidbody.velocity.z);
        // 在进入 GlidingState 状态时进行初始化

    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        InputEvent.Instance.onLeftTriggerUp += TriggerUp;
        InputEvent.Instance.onRightTriggerUp += TriggerUp;
        InputEvent.Instance.onLeftTriggerDown -= TriggerUp;
        InputEvent.Instance.onRightTriggerDown -= TriggerUp;

    Rigidbody rigidbody = _sm.player.GetComponent<Rigidbody>();
    Vector3 currentVelocity = rigidbody.velocity;
    if (currentVelocity.magnitude > maxSpeed)
    {
            // 计算速度在单位方向上的向量
            Vector3 normalizedVelocity = currentVelocity.normalized;

            // 将速度限制在阈值内
            rigidbody.velocity = normalizedVelocity * maxSpeed;
    }
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
        // 给物体施加持续的浮力
        _sm.player.GetComponent<Rigidbody>().AddForce(Vector3.up * glideForce, ForceMode.Acceleration);
        AddForce();

        Rigidbody rigidbody = _sm.player.GetComponent<Rigidbody>();
        // 给物体施加重力
        rigidbody.AddForce(_sm.player.forward * glideFoward, ForceMode.Acceleration);
    }

    public void HandleCollisionEnter(Collision collision)
    {
        // 处理碰撞事件
        if (collision.gameObject.CompareTag("Ground"))
        {
            _sm.player.GetComponent<ActionBasedContinuousTurnProvider>().enabled = true;
            _sm.player.GetComponent<ActionBasedContinuousMoveProvider>().enabled = true;
            playerStateMachine.ChangeState(_sm.idleState);
        }
    }
    public void HandleCollisionExit(Collision collision)
    {
        // 处理碰撞退出事件
    }


    public void TriggerUp()
    {
        _sm.player.GetComponent<ActionBasedContinuousTurnProvider>().enabled = true;
        _sm.player.GetComponent<ActionBasedContinuousMoveProvider>().enabled = true;
        playerStateMachine.ChangeState(_sm.flappingState);
    }

    public void AddForce()
    {
        Rigidbody rigidbody = _sm.player.GetComponent<Rigidbody>();
        Vector3 leftPos = leftController.position;
        Vector3 rightPos = rightController.position;

        // 计算两者的垂直差异
        float verticalDifference = rightPos.y - leftPos.y;

        // 如果差异超过阈值
        if (verticalDifference > threshold)
        {
            Vector3 forceDirection = Vector3.Cross(rigidbody.velocity.normalized, Vector3.up);
            rigidbody.AddForce(forceDirection * forceMagnitude, ForceMode.Force);
            float rotationThisFrame = -rotationSpeed * Time.deltaTime;

            // 绕 Y 轴旋转
            _sm.player.transform.Rotate(Vector3.up, rotationThisFrame);

        }
        if(verticalDifference < -threshold) 
        {
            Vector3 forceDirection = Vector3.Cross(rigidbody.velocity.normalized, Vector3.up);
            rigidbody.AddForce(-forceDirection * forceMagnitude, ForceMode.Force);
            float rotationThisFrame = rotationSpeed * Time.deltaTime;

            // 绕 Y 轴旋转
            _sm.player.transform.Rotate(Vector3.up, rotationThisFrame);
        }
    }
}
