using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelEnd : MonoBehaviour
{
    public AudioManager audioManager; // 引用 AudioManager

    // 这个方法在碰撞发生时会被调用
    private void OnTriggerEnter(Collider other)
    {
        // 检查碰撞到的物体是否是玩家
        if (other.CompareTag("Player"))
        {
            // 启动协程，先触发眨眼效果，再延迟切换关卡
            StartCoroutine(BlinkAndChangeLevel());
        }
    }

    private IEnumerator BlinkAndChangeLevel()
    {
        // 触发眨眼效果和开始淡出 BGM
        audioManager.StopBGM();
        BlinkEffect.Instance.TriggerBlink();


        // 等待 BGM 的淡出时间
        yield return new WaitForSeconds(0.3f);

        // 切换到下一个关卡（假设关卡是按照数字命名的，例如 Level1、Level2 等）
        // 注意：确保在场景管理器中添加了相应的关卡，并且它们按顺序排列
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
