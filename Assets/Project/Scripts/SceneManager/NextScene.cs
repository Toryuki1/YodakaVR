using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    // 这个方法在碰撞发生时会被调用
    public void LoadNextScene()
    {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
}
