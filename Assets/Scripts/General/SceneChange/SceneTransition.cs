using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private Image image;
    private float duration = 1f;

    private void OnEnable()
    {
        GameEvents.SceneFadeIn += FadeIn;
        GameEvents.SceneFadeOut += FadeOut;
    }

    private void OnDisable()
    {
        GameEvents.SceneFadeIn -= FadeIn;
        GameEvents.SceneFadeOut -= FadeOut;
    }

    private void FadeIn()
    {
        StartCoroutine(FadeInRoutine());
    }

    private void FadeOut()
    {
        StartCoroutine(FadeOutRoutine());
    }

    private IEnumerator FadeInRoutine()
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float normalizedTime = elapsed / duration;
            Color color = image.color;
            color.a = normalizedTime;
            image.color = color;
            yield return null;
        }
    }

    private IEnumerator FadeOutRoutine()
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float normalizedTime = elapsed / duration;
            Color color = image.color;
            color.a = 1f - normalizedTime;
            image.color = color;
            yield return null;
        }
    }
}
