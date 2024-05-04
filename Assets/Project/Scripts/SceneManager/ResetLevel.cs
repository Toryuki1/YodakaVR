using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetLevel : MonoBehaviour
{
    // 这个方法在碰撞发生时会被调用
    private void OnTriggerEnter(Collider other)
    {
        // 检查碰撞到的物体是否是玩家
        if (other.CompareTag("Player"))
        {
            // 重新加载当前关卡
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
