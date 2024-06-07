using UnityEngine;

public class ApplyContinuousIncreasingDownwardForce : MonoBehaviour
{
    public float initialForce = 80f; // 初始力
    public float maxForce = 250f; // 最大力
    public float forceIncreaseRate = 10f; // 力增加的速率

    private float currentForce;

    private void Start()
    {
        // 初始化当前力为初始力
        currentForce = initialForce;
    }

    private void OnTriggerStay(Collider other)
    {
        // 检查触发的对象是否有标签 "Player"
        if (other.gameObject.CompareTag("Player"))
        {
            // 获取触发对象的Rigidbody组件
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();

            // 确认Rigidbody组件存在
            if (rb != null)
            {
                // 施加当前的力
                rb.AddForce(Vector3.down * currentForce * Time.deltaTime, ForceMode.Acceleration);

                // 增加当前力，但不超过最大力
                currentForce = Mathf.Min(currentForce + forceIncreaseRate * Time.deltaTime, maxForce);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 当玩家离开触发区时，重置当前力
        if (other.gameObject.CompareTag("Player"))
        {
            currentForce = initialForce;
        }
    }
}
