using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

public class FogOfWarController : MonoBehaviour
{
    [SerializeField] Material FogMaterial;
    [SerializeField] Animator Animator;
    [SerializeField] float FadeSpeed;
    [SerializeField] float FadedValue;

    float DefaultFogCoverage;
    bool DefaultSide;
    bool IsOnTheRight = false;
    
    private const string FOG_COVERAGE_LEFT = "_FogCoverageLeft";
    private const string FOG_COVERAGE_RIGHT = "_FogCoverageRight";
    private const string SPRITE_MASK_MOVE_LEFT = "MovingLeft";
    private const string SPRITE_MASK_MOVE_RIGHT = "MovingRight";
    private const string IS_FOG_ON_LEFT = "_IsOnLeft";

    private void OnDisable()
    {
        FogMaterial.SetFloat(FOG_COVERAGE_LEFT, DefaultFogCoverage);
        FogMaterial.SetInt(IS_FOG_ON_LEFT, DefaultSide ? 1 : 0);
    }

    private void Start()
    {
        DefaultFogCoverage = FogMaterial.GetFloat(FOG_COVERAGE_LEFT);
        IsOnTheRight = FogMaterial.GetInt(IS_FOG_ON_LEFT) == 1 ? true : false;
        DefaultSide = IsOnTheRight;
    }

    [ContextMenu("Fade")]
    public void DissolveAndMoveFog()
    {
        DissolveFog().OnComplete(() =>
        {
            MoveFog();
        });
    }

    private TweenerCore<float, float, FloatOptions> DissolveFog()
    {
        string leftOrRight;
        if (IsOnTheRight)
        {
            Animator.SetBool(SPRITE_MASK_MOVE_RIGHT, false);
            Animator.SetBool(SPRITE_MASK_MOVE_LEFT, true);
            leftOrRight = FOG_COVERAGE_RIGHT;
        }
        else
        {
            Animator.SetBool(SPRITE_MASK_MOVE_LEFT, false);
            Animator.SetBool(SPRITE_MASK_MOVE_RIGHT, true);
            leftOrRight = FOG_COVERAGE_LEFT;
        }
        FogMaterial.SetInt(IS_FOG_ON_LEFT, IsOnTheRight ? 1 : 0);
        return DOTween.To(
                        () => FogMaterial.GetFloat(FOG_COVERAGE_LEFT),
                        value => FogMaterial.SetFloat(leftOrRight, value),
                        FadedValue,
                        FadeSpeed
                    );
    }

    private void MoveFog()
    {
        IsOnTheRight = !IsOnTheRight;
        //FogMaterial.SetFloat(FOG_COVERAGE,
    }

    private void EndAnimations()
    {
        Animator.SetBool(SPRITE_MASK_MOVE_LEFT, false);
        Animator.SetBool(SPRITE_MASK_MOVE_RIGHT, false);
    }
}
