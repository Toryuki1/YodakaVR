using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class NextScene : MonoBehaviour
{
    public AudioManager audioManager; // 引用 AudioManager

    // 这个方法在碰撞发生时会被调用
    public void LoadNextScene()
    {
        // 启动协程来处理眨眼效果和BGM淡出
        StartCoroutine(BlinkAndChangeLevel());
    }

    private IEnumerator BlinkAndChangeLevel()
    {
        // 触发眨眼效果和开始淡出 BGM
        audioManager.StopBGM();
        BlinkEffect.Instance.TriggerBlink();


        // 等待 BGM 的淡出时间
        yield return new WaitForSeconds(audioManager.fadeDuration - 0.5f);

        // 切换到下一个关卡（假设关卡是按照数字命名的，例如 Level1、Level2 等）
        // 注意：确保在场景管理器中添加了相应的关卡，并且它们按顺序排列
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
