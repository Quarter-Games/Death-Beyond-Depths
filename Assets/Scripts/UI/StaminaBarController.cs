using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBarController : MonoBehaviour
{
    [SerializeField] Image Image;
    [SerializeField] PlayerCharacterController Player;
    [SerializeField, Min(0)] float BarSpeed = 0.3f;
    [SerializeField] Ease EaseSetting;

    float MaxStamina;
    bool isSubscribedToPlayer = false;
    private Tween FillTween;

    private void OnValidate()
    {
        Image = GetComponent<Image>();
    }
    private void OnEnable()
    {
        if (Player != null)
        {
            isSubscribedToPlayer = true;
            Player.Stamina.OnStaminaChanged += UpdateStaminaBar;
        }
    }

    private void OnDisable()
    {
        if (Player != null)
            Player.Stamina.OnStaminaChanged -= UpdateStaminaBar;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!isSubscribedToPlayer)
        {
            Player.Stamina.OnStaminaChanged += UpdateStaminaBar;
        }
        MaxStamina = Player.Stamina.MaxStamina;
        Image.fillAmount = 1;
    }
    public void UpdateStaminaBar()
    {
        if (Player == null) return;
        float targetFill = Mathf.Clamp01(Player.Stamina.CurrentStamina / MaxStamina);
        FillTween?.Kill();
        FillTween = Image.DOFillAmount(targetFill, BarSpeed).SetEase(EaseSetting);
    }
}
