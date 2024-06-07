using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public AudioSource bgmSource;
    public float fadeDuration = 1.0f;

    private void Start()
    {
        // 开始时渐入
        StartCoroutine(FadeIn(bgmSource, fadeDuration));
    }

    public void PlayBGM(AudioClip clip)
    {
        bgmSource.clip = clip;
        bgmSource.Play();
        StartCoroutine(FadeIn(bgmSource, fadeDuration));
    }

    public void StopBGM()
    {
        StartCoroutine(FadeOut(bgmSource, fadeDuration));
    }

    private IEnumerator FadeIn(AudioSource audioSource, float duration)
    {
        float startVolume = 0f;
        audioSource.volume = startVolume;
        float targetVolume = 1f;

        while (audioSource.volume < targetVolume)
        {
            audioSource.volume += Time.deltaTime / duration;
            yield return null;
        }

        audioSource.volume = targetVolume;
    }

    private IEnumerator FadeOut(AudioSource audioSource, float duration)
    {
        float startVolume = audioSource.volume;
        
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / duration;
            yield return null;
        }

        audioSource.volume = 0;
        audioSource.Stop();
    }
}
