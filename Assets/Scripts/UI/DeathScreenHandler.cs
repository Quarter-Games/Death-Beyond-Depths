using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DeathScreenHandler : MonoBehaviour
{
    [SerializeField] CanvasGroup ScreenCanvasElements;
    [SerializeField] float FadeInDuration = 1f;

    private void OnValidate()
    {
        if (ScreenCanvasElements != null) gameObject.TryGetComponent(out ScreenCanvasElements);
    }

    private void OnEnable()
    {
        PlayerCharacterController.OnPlayerDeath += ShowScreenOverTime;
    }

    private void OnDisable()
    {
        PlayerCharacterController.OnPlayerDeath -= ShowScreenOverTime;
    }

    private void Start()
    {
        if (ScreenCanvasElements != null)
        {
            ScreenCanvasElements.alpha = 0f;
            ScreenCanvasElements.interactable = false;
        }
    }

    private void ShowScreenOverTime()
    {
        if (ScreenCanvasElements != null)
        {
            StartCoroutine(FadeInCanvas());
        }
    }

    private IEnumerator FadeInCanvas()
    {
        float elapsedTime = 0f;
        while (elapsedTime < FadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            ScreenCanvasElements.alpha = Mathf.Lerp(0f, 1f, elapsedTime / FadeInDuration);
            yield return null;
        }
        ScreenCanvasElements.interactable = true;
        ScreenCanvasElements.alpha = 1f;
    }
}
