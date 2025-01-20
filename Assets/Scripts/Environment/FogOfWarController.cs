using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections;

public class FogOfWarController : MonoBehaviour
{
    [SerializeField] Material FogMaterial;
    [SerializeField] float FadeSpeed;
    [SerializeField] float FadedValue;
    [Header("Sprite Mask")]
    [SerializeField] Transform SpriteMaskTransform;
    [SerializeField] float SpriteMaskMoveSpeed = 5f;
    [SerializeField] float OutPositionX;
    [SerializeField] float InPositionX;

    float DefaultFogCoverage;
    bool DefaultSide;
    bool IsPlayerFacingRight = false;
    private const string FOG_COVERAGE_LEFT = "_FogCoverageLeft";
    private const string FOG_COVERAGE_RIGHT = "_FogCoverageRight";
    private const string SPRITE_MASK_MOVE_LEFT = "MovingLeft";
    private const string SPRITE_MASK_MOVE_RIGHT = "MovingRight";
    private const string IS_FOG_ON_LEFT = "_IsOnLeft";

    private void OnEnable()
    {
        PlayerCharacterController.OnFlip += DissolveAndMoveFog;
    }

    private void OnDisable()
    {
        PlayerCharacterController.OnFlip -= DissolveAndMoveFog;
        FogMaterial.SetFloat(FOG_COVERAGE_LEFT, DefaultFogCoverage);
        FogMaterial.SetFloat(FOG_COVERAGE_RIGHT, DefaultFogCoverage / 2);
        FogMaterial.SetInt(IS_FOG_ON_LEFT, DefaultSide ? 1 : 0);
    }


    private void Start()
    {
        DefaultFogCoverage = FogMaterial.GetFloat(FOG_COVERAGE_LEFT);
        IsPlayerFacingRight = FogMaterial.GetInt(IS_FOG_ON_LEFT) == 1 ? true : false;
        DefaultSide = IsPlayerFacingRight;
    }

    [ContextMenu("Fade")]
    public void DissolveAndMoveFog(bool isFacingRight)
    {
        IsPlayerFacingRight = isFacingRight;
        Sequence fogSequence = DOTween.Sequence();
        fogSequence.Append(DissolveFog());
        MoveSpriteMask();
    }

    private TweenerCore<float, float, FloatOptions> DissolveFog()
    {
        string fogCoverageKey = IsPlayerFacingRight ? FOG_COVERAGE_RIGHT : FOG_COVERAGE_LEFT;
        FogMaterial.SetFloat(IsPlayerFacingRight ? FOG_COVERAGE_LEFT : FOG_COVERAGE_RIGHT, IsPlayerFacingRight ? DefaultFogCoverage : DefaultFogCoverage / 2);
        FogMaterial.SetInt(IS_FOG_ON_LEFT, IsPlayerFacingRight ? 1 : 0);
        
        return DOTween.To(
            () => FogMaterial.GetFloat(fogCoverageKey),
            value => FogMaterial.SetFloat(fogCoverageKey, value),
            FadedValue,
            FadeSpeed
        );
    }

    private void MoveSpriteMask()
    {
        Vector3 inPosition = new Vector3(IsPlayerFacingRight ? InPositionX : -InPositionX, SpriteMaskTransform.position.y, SpriteMaskTransform.position.z);
        SpriteMaskTransform.position = new Vector3(IsPlayerFacingRight ? InPositionX : OutPositionX, SpriteMaskTransform.position.y, SpriteMaskTransform.position.z);
    }
}
