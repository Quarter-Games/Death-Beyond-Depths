using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageVignetteController : MonoBehaviour
{
    [SerializeField] Material ScreenDamageMat;
    [SerializeField] private float MinVignetteRadius = -0.75f;
    [SerializeField] private float MaxVignetteRadius = -0.001f;
    [SerializeField] private float MinIntensity = -1f;
    [SerializeField] private float MaxIntensity = 1f;
    [SerializeField] private float VignetteSpeed = 1f; // Higher = faster
    private Coroutine ScreenDamageTask;

    List<EnemyAI> Enemies = new();

    public static DamageVignetteController Instance;

    private void OnEnable()
    {
        ChaseState.OnSeenPlayer += SeenByEnemy;
        AlertState.OnLosingPlayer += EnemyLostPlayer;
    }

    private void OnDisable()
    {
        ScreenDamageMat.SetFloat("_Vignette_Radius", 1);
        ChaseState.OnSeenPlayer -= SeenByEnemy;
        AlertState.OnLosingPlayer -= EnemyLostPlayer;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        Enemies = new();
    }

    private void Start()
    {
        ScreenDamageMat.SetFloat("_Vignette_Radius", 1);
    }

    [ContextMenu("Vignette")]
    public void Test()
    {
        ScreenDamageEffect(0.5f);
    }

    private void ScreenDamageEffect(float intensity)
    {
        if (ScreenDamageTask != null)
            StopCoroutine(ScreenDamageTask);

        ScreenDamageTask = StartCoroutine(ScreenDamage(intensity));
    }
    private IEnumerator ScreenDamage(float intensity)
    {
        var targetRadius = Remap(intensity, MinIntensity, MaxIntensity, MinVignetteRadius, MaxVignetteRadius);
        var curRadius = 1f;
        for (float t = 0; Mathf.Abs(curRadius - targetRadius) > 0.001f; t += Time.deltaTime * VignetteSpeed)
        {
            curRadius = Mathf.Lerp(1, targetRadius, t);
            ScreenDamageMat.SetFloat("_Vignette_Radius", curRadius);
            yield return null;
        }
        curRadius = targetRadius;
        ScreenDamageMat.SetFloat("_Vignette_Radius", curRadius);
        for (float t = 0; Mathf.Abs(curRadius - 1f) > 0.001f; t += Time.deltaTime * VignetteSpeed)
        {
            curRadius = Mathf.Lerp(targetRadius, 1, t);
            ScreenDamageMat.SetFloat("_Vignette_Radius", curRadius);
            yield return null;
        }
        curRadius = 1f;
        ScreenDamageMat.SetFloat("_Vignette_Radius", curRadius);
    }

    private float Remap(float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        return Mathf.Lerp(toMin, toMax, Mathf.InverseLerp(fromMin, fromMax, value));
    }

    public static class SpecialEffects
    {
        public static void ScreenDamageEffect(float intensity) => Instance.ScreenDamageEffect(intensity);
    }

    private IEnumerator StartDetectionVignette()
    {
        var targetRadius = Remap(0.5f, -0f, 1, -0.35f, -0.001f);
        var curRadius = 1f;
        for (float t = 0; curRadius != targetRadius; t += Time.deltaTime - 0.01f)
        {
            curRadius = Mathf.Clamp(Mathf.Lerp(1, 0.1f, t), 1, 0.1f);
            ScreenDamageMat.SetFloat("_Vignette_Radius", 0.35f);
            yield return null;
        }
    }

    public void SeenByEnemy(EnemyAI enemy)
    {
        if(Enemies.Contains(enemy)) return;
        Enemies.Add(enemy);
        if (Enemies.Count == 1)
        {
            StartCoroutine(StartDetectionVignette());
        }
    }

    public void EnemyLostPlayer(EnemyAI enemy)
    {
        Enemies.Remove(enemy);
        if (Enemies.Count == 0)
        {

        }
    }
}
