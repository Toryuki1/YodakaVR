using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;

public class IdleState : PlayerState
{
    private MovementSM _sm;
    private float flapForce = 1f;
    private float upForce = 3f;
    private Vector3 flapForceDirection = new Vector3(0f, 4f, 1f).normalized;
    private Vector3 upWard = new Vector3(0f, 2f, 0f).normalized;

    private Transform leftController;
    private Transform rightController;
    private Transform player;

    private Vector3 previousPosition;
    private bool leftSwing = false;
    private bool rightSwing = false;
    private float swingDelay = 0.6f;
    private float lastSwingTime = 0f;
    private Vector3 lastLeftPosition;
    private Vector3 lastRightPosition;
    private double PositionThreshold = 0.02f; // 挥动检测阈值
    
    private float gravity = 0.9f;

    public IdleState(MovementSM stateMachine) : base("IdleState", stateMachine)
    {
        _sm = stateMachine;
    }

    public override void EnterState()
    {
        base.EnterState();
        leftController = _sm.leftController;
        rightController = _sm.rightController;
        lastLeftPosition = leftController.transform.position;
        lastRightPosition = rightController.transform.position;

    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        Rigidbody rigidbody = _sm.player.GetComponent<Rigidbody>();
        rigidbody.AddForce(Vector3.down * gravity, ForceMode.Acceleration);
        flapForceDirection = _sm.player.forward;
        // 检测挥动触发 Flap
        DetectSwing();
    }


    private void Flap()
    {
        // 使用指定的力方向和大小给物体施加力量
        _sm.player.GetComponent<Rigidbody>().AddForce(flapForceDirection * flapForce, ForceMode.VelocityChange);
        _sm.player.GetComponent<Rigidbody>().AddForce(upWard * upForce, ForceMode.VelocityChange);
    }


    // 挥动检测功能
    private void DetectSwing()
    {
        // 计算左右手柄在x轴上的旋转变化
        float leftPositionDelta = lastLeftPosition.y - leftController.transform.position.y;
        float rightPositionDelta = lastRightPosition.y - rightController.transform.position.y;

        // 如果旋转变化超过阈值，则认为是挥动
        leftSwing = leftPositionDelta > PositionThreshold;
        rightSwing = rightPositionDelta > PositionThreshold;

        // 更新上一帧的旋转信息
        lastLeftPosition.y = leftController.transform.position.y;
        lastRightPosition.y = rightController.transform.position.y;

        // 如果左右手柄都向下挥动，并且间隔超过0.6秒，则触发flap函数
        if (leftSwing && rightSwing && Time.time - lastSwingTime >= swingDelay)
        {
            Flap();
            lastSwingTime = Time.time;
            // 启动协程来延迟切换状态
            _sm.StartCoroutine(DelayedStateChange());
        }
    }

    // 延迟状态切换的协程
    private IEnumerator DelayedStateChange()
    {
        // 等待0.5秒
        yield return new WaitForSeconds(0.5f);
        
        // 执行状态切换
        playerStateMachine.ChangeState(_sm.flappingState);
    }
}
