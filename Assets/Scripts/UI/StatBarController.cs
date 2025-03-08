using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StatBarController : MonoBehaviour
{
    [SerializeField] Image Image;
    [SerializeField] PlayerCharacterController Player;
    [SerializeField, Min(0)] float BarSpeed = 0.3f;
    [SerializeField] Ease EaseSetting;
    [SerializeField] GameObject HUD;

    float MaxHp;
    bool isSubscribedToPlayer = false;
    private Tween FillTween;
    private Tween BeatTween;

    private void OnValidate()
    {
        Image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        if (Player != null)
        {
            isSubscribedToPlayer = true;
            Player.stats.OnHPChanged += UpdateHPBar;
        }
    }

    private void OnDisable()
    {
        if (Player != null)
            Player.stats.OnHPChanged -= UpdateHPBar;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!isSubscribedToPlayer)
        {
            Player.stats.OnHPChanged += UpdateHPBar;
        }
        MaxHp = Player.stats.HP;
        Image.fillAmount = 1;
        StartHUDPulse();
    }

    private void StartHUDPulse()
    {
        if (HUD == null) return;

        // Kill any existing tween before starting a new one
        BeatTween?.Kill();

        // Create a Yoyo loop that makes the HUD scale up and down continuously
        BeatTween = HUD.transform
            .DOScale(1.01f, 0.15f)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    private void UpdateHPBar()
    {
        if (Player == null) return;
        float targetFill = Mathf.Clamp01(Player.stats.HP / MaxHp);
        FillTween?.Kill();
        FillTween = Image.DOFillAmount(targetFill, BarSpeed).SetEase(EaseSetting);
    }
}
