using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoSingleton<ScreenFader>
{
    [SerializeField] Image fadeImage = null;
    
    public void ScreenFade(float fadeTime, float waitTime = 0f, System.Action onCompleteFadeOut = null, System.Action onCompleteFadeIn = null)
    {
        StartCoroutine(FadeCorutine(fadeTime, waitTime, onCompleteFadeOut, onCompleteFadeIn));
    }

    private IEnumerator FadeCorutine(float fadeTime, float waitTime = 0f, System.Action onCompleteFadeOut = null, System.Action onCompleteFadeIn = null)
    {
        float timer = 0f;

        Time.timeScale = 0;
        while (timer <= 1f)
        {
            timer += Time.unscaledDeltaTime / (fadeTime / 2);
            fadeImage.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, timer));
            yield return null;
        }

        onCompleteFadeOut?.Invoke();
        yield return new WaitForSecondsRealtime(waitTime);

        timer = 0f;

        while (timer <= 1f)
        {
            timer += Time.unscaledDeltaTime / (fadeTime / 2);
            fadeImage.color = new Color(0, 0, 0, Mathf.Lerp(1, 0, timer));
            yield return null;
        }

        onCompleteFadeIn?.Invoke();
        Time.timeScale = 1;

        yield return null;
    }
}
