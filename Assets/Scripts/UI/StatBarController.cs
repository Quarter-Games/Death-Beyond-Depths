using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StatBarController : MonoBehaviour
{
    [SerializeField] Image Image;
    [SerializeField] PlayerCharacterController Player;
    [SerializeField, Min(0)] float BarSpeed = 0.3f;
    [SerializeField] Ease EaseSetting;

    float MaxHp;
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
        Image.fillAmount = MaxHp;
    }

    private void UpdateHPBar()
    {
        if (Player == null) return;
        float targetFill = Mathf.Clamp01(Player.stats.HP / MaxHp);
        FillTween?.Kill();
        FillTween = Image.DOFillAmount(targetFill, BarSpeed).SetEase(EaseSetting);
    }
}
