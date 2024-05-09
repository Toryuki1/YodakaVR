using UnityEngine;

public class FlappingState : PlayerState
{
    private MovementSM _sm;
    private float flapForce = 1.1f;
    private float upForce = 0.3f;
    private Vector3 flapForceDirection = new Vector3(1f, 2f, 0f).normalized;
    private Vector3 upWard = new Vector3(0f, 2f, 0f).normalized;
    private float gravity = 0.8f;
    public float maxSpeed = 4.5f;

    private Transform leftController;
    private Transform rightController;
    private Transform player;


    private bool leftSwing = false;
    private bool rightSwing = false;
    private float swingDelay = 0.6f;
    private float lastSwingTime = 0f;
    private Quaternion lastLeftRotation;
    private Quaternion lastRightRotation;
    private float xRotationThreshold = 12f; // 挥动检测阈值

    public float forceMagnitude = 0.6f;

    public FlappingState(MovementSM stateMachine) : base("FlappingState", stateMachine)
    {
        _sm = stateMachine;
    }

    public override void EnterState()
    {
        base.EnterState();

        leftController = _sm.leftController;
        rightController = _sm.rightController;
        lastLeftRotation = leftController.rotation;
        lastRightRotation = rightController.rotation;

        
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        flapForceDirection = _sm.player.forward;


        Rigidbody rigidbody = _sm.player.GetComponent<Rigidbody>();
        Vector3 currentVelocity = rigidbody.velocity;

        if (currentVelocity.magnitude > maxSpeed)
        {
            // 计算速度在单位方向上的向量
            Vector3 normalizedVelocity = currentVelocity.normalized;

            // 将速度限制在阈值内
            rigidbody.velocity = normalizedVelocity * maxSpeed;
        }

        // 在此处添加 flipping 状态下的逻辑

        InputEvent.Instance.onLeftTriggerDown += TriggerDown;
        InputEvent.Instance.onRightTriggerDown += TriggerDown;
        InputEvent.Instance.onRightTriggerUp -= TriggerDown;
        InputEvent.Instance.onLeftTriggerUp -= TriggerDown;
        DetectSwing();
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
        Rigidbody rigidbody = _sm.player.GetComponent<Rigidbody>();
        // 给物体施加重力
        rigidbody.AddForce(Vector3.down * gravity, ForceMode.Acceleration);

        Vector3 velocity = rigidbody.velocity;
        Vector3 oppositeForce = -velocity.normalized * forceMagnitude;
        rigidbody.AddForce(oppositeForce, ForceMode.Force);

    }


    private void Flap()
    {
        Rigidbody rigidbody = _sm.player.GetComponent<Rigidbody>();
        rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0.8f, rigidbody.velocity.z);
        // 给物体施加向上的力量
        _sm.player.GetComponent<Rigidbody>().AddForce(flapForceDirection * flapForce, ForceMode.VelocityChange);
        _sm.player.GetComponent<Rigidbody>().AddForce(upWard * upForce, ForceMode.VelocityChange);
    }

    public void HandleCollisionEnter(Collision collision)
    {
        // 处理碰撞事件
        if (collision.gameObject.CompareTag("Ground"))
        {
            playerStateMachine.ChangeState(_sm.idleState);
        }
    }

    public void HandleCollisionExit(Collision collision)
    {
        // 处理碰撞退出事件
    }

    private void DetectSwing()
    {
        // 计算左右手柄在x轴上的旋转变化
        float leftRotationDelta = Quaternion.Angle(lastLeftRotation, leftController.rotation);
        float rightRotationDelta = Quaternion.Angle(lastRightRotation, rightController.rotation);

        // 如果旋转变化超过阈值，则认为是挥动
        leftSwing = leftRotationDelta > xRotationThreshold;
        rightSwing = rightRotationDelta > xRotationThreshold;

        // 更新上一帧的旋转信息
        lastLeftRotation = leftController.rotation;
        lastRightRotation = rightController.rotation;

        // 如果左右手柄都向下挥动，并且间隔超过0.6秒，则触发flap函数
        if (leftSwing && rightSwing && Time.time - lastSwingTime >= swingDelay)
        {
            Flap();
            lastSwingTime = Time.time;
        }
    }
    public void TriggerDown()
    {
        playerStateMachine.ChangeState(_sm.glidingState);
    }
}
