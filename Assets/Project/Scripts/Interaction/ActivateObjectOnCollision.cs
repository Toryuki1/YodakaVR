using UnityEngine;

public class ActivateObjectOnCollision : MonoBehaviour
{
    public GameObject objectToActivate; // 需要激活的物体

    private void OnCollisionEnter(Collision collision)
    {
        // 检查碰撞到的对象是否为玩家
        if (collision.gameObject.CompareTag("Player"))
        {
            // 激活另一个物体
            if (objectToActivate != null)
            {
                objectToActivate.SetActive(true);
            }
            else
            {
                Debug.LogWarning("Object to activate is not assigned!");
            }
        }
    }
}
