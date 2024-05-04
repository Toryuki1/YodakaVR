using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    // 这个方法在碰撞发生时会被调用
    private void OnTriggerEnter(Collider other)
    {
        // 检查碰撞到的物体是否是玩家
        if (other.CompareTag("Player"))
        {
            // 切换到下一个关卡（假设关卡是按照数字命名的，例如 Level1、Level2 等）
            // 注意：确保在场景管理器中添加了相应的关卡，并且它们按顺序排列
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
