using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class PlayerTextController : MonoBehaviour
{
    [SerializeField] float TextDuration = 2.5f;
    [SerializeField] TMP_Text Text;

    float DefaultTextDuration;
    float TimeTextIsActive = 0f;
    bool IsFirstDisable = true;

    private void OnEnable()
    {
        TimeTextIsActive = 0f;
        PlayerCharacterController.OnFlip += FlipText;
    }

    private void OnDisable()
    {
        PlayerCharacterController.OnFlip -= FlipText;
        if (IsFirstDisable) return;
        IsFirstDisable = false;
        TextDuration = DefaultTextDuration;
    }

    private void FlipText(bool isFacingRight)
    {
        float rotation = isFacingRight ? 180f : 0;
        transform.rotation = Quaternion.Euler(0f, rotation, 0f);
    }

    private void Update()
    {
        if (TimeTextIsActive >= TextDuration)
        {
            gameObject.SetActive(false);
        }
        TimeTextIsActive += Time.deltaTime;
    }

    public void SetText(string text, Color textColor)
    {
        Text.text = text;
        Text.color = textColor;
    }

    public void SetTextTimer(float newDuration)
    {
        DefaultTextDuration = TextDuration;
        TextDuration = newDuration;
    }
}
