using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

public class FogOfWarController : MonoBehaviour
{
    [SerializeField] Material FogMaterial;
    [SerializeField] float FadeSpeed;

    float DefaultFogCoverage;
    bool IsOnTheRight = false;
    
    private const string FOG_COVERAGE = "FogCoverage";

    private void Start()
    {
        DefaultFogCoverage = FogMaterial.GetFloat(FOG_COVERAGE);
    }

    public void DissolveAndMoveFog()
    {
        DissolveFog().OnComplete(() =>
        {
            MoveFog();
        });
    }

    private TweenerCore<float, float, FloatOptions> DissolveFog()
    {
        return DOTween.To(
                        () => FogMaterial.GetFloat(FOG_COVERAGE),
                        value => FogMaterial.SetFloat(FOG_COVERAGE, value),
                        0,
                        FadeSpeed
                    );
    }

    private void MoveFog()
    {
        IsOnTheRight = !IsOnTheRight;
    }

    private void ResetFog()
    {
        FogMaterial.SetFloat(FOG_COVERAGE, DefaultFogCoverage);
    }
}
