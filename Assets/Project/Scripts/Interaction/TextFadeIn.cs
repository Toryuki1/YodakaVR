using UnityEngine;
using TMPro;
using System.Collections;

public class TextFadeIn : MonoBehaviour
{
    public float fadeInSpeed = 0.5f; // 淡入速度
    public float fadeOutSpeed = 0.5f; // 淡出速度
    public float displayDuration = 2f; // 显示时间
    private TextMeshPro textMeshPro;
    private BoxCollider boxCollider;

    void Start()
    {
        textMeshPro = GetComponent<TextMeshPro>();
        boxCollider = GetComponent<BoxCollider>();
        textMeshPro.alpha = 0f; // 将文字的透明度初始化为0
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(FadeInAndOut());
        }
    }

    IEnumerator FadeInAndOut()
    {
        // 淡入
        while (textMeshPro.alpha < 1f)
        {
            textMeshPro.alpha += Time.deltaTime * fadeInSpeed;
            yield return null;
        }

        // 显示文本
        yield return new WaitForSeconds(displayDuration);

        // 淡出
        while (textMeshPro.alpha > 0f)
        {
            textMeshPro.alpha -= Time.deltaTime * fadeOutSpeed;
            boxCollider.enabled = false;
            yield return null;
        }
    }
}
