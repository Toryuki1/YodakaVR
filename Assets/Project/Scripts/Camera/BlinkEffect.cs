using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BlinkEffect : MonoBehaviour
{
    public static BlinkEffect Instance { get; private set; }

    public Image blinkImage;  // 引用黑色Image
    public float blinkDuration = 2.5f;  // 眨眼持续时间（秒）

    private bool isBlinking;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // 确保对象在场景切换时不被销毁
        }
        else
        {
            Destroy(gameObject);  // 如果已经有一个实例存在，则销毁新的实例
        }
    }

    void Start()
    {
        if (blinkImage == null)
        {
            Debug.LogError("BlinkImage is not assigned.");
            return;
        }
        blinkImage.color = new Color(0, 0, 0, 0);  // 初始时是透明的
    }

    public void TriggerBlink()
    {
        if (!isBlinking)
        {
            StartCoroutine(Blink());
        }
    }

    private IEnumerator Blink()
    {
        isBlinking = true;

        // 渐变到黑色（闭眼）
        yield return FadeTo(1, blinkDuration / 2);

        // 渐变到透明（睁眼）
        yield return FadeTo(0, blinkDuration / 2);

        isBlinking = false;
    }

    private IEnumerator FadeTo(float targetAlpha, float duration)
    {
        float startAlpha = blinkImage.color.a;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            blinkImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        blinkImage.color = new Color(0, 0, 0, targetAlpha);
    }
}
