using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverOverlay : MonoBehaviour
{
    public Image redOverlay;
    public GameObject overlayRoot; 
    public float fadeDuration = 0.5f;

    private Coroutine fadeCoroutine;

    void Start()
    {
        redOverlay.color = new Color(1f, 0f, 0f, 0f);
    }

    public void ShowGameOver()
    {
        overlayRoot.SetActive(true);
        if(fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeInRed());
    }

    IEnumerator FadeInRed()
    {
        redOverlay.color = new Color(0.5f, 0f, 0f, 0.3f);
        yield return null;

        float timer = 0f;
        float startAlpha = 0.3f;
        float targetAlpha = 1.0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, timer / fadeDuration);
            redOverlay.color = new Color(0.5f, 0f, 0f, alpha);
            yield return null;
        }
    }

    public void HideOverlay()
    {
        Debug.Log("Hiding Overlay");
        if(fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        overlayRoot.SetActive(false);
    }
}

