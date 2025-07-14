using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DamageVignetteController : MonoBehaviour
{
    [SerializeField] Material ScreenDamageMat;
    [SerializeField] Color DamageColor;
    [SerializeField] Color StealthColor;
    [SerializeField] float StealthMaxIntensity;
    [SerializeField] private float MinVignetteRadius = -0.75f;
    [SerializeField] private float MaxVignetteRadius = -0.001f;
    [SerializeField] private float MinIntensity = -1f;
    [SerializeField] private float MaxIntensity = 1f;
    [SerializeField] private float VignetteSpeed = 1f;
    [SerializeField, Tooltip("The larger this number, the more noticeable the change on every hit")]
    private float VignetteStrengthModifier = 2f;
    private Coroutine ScreenDamageTask;

    List<EnemyAI> Enemies = new();

    private const string VIGNETTE_RADIUS = "_Amounth";
    private const string VIGNETTE_COLOR = "_Color";
    public static DamageVignetteController Instance;

    private void OnEnable()
    {
        ScreenDamageMat.SetColor(VIGNETTE_COLOR, DamageColor);
        //ChaseState.OnSeenPlayer += SeenByEnemy;
        //AlertState.OnLosingPlayer += EnemyLostPlayer;
    }

    private void OnDisable()
    {
        ScreenDamageMat.SetFloat(VIGNETTE_RADIUS, 0);
        ScreenDamageMat.SetColor(VIGNETTE_COLOR, DamageColor);
        //ChaseState.OnSeenPlayer -= SeenByEnemy;
        //AlertState.OnLosingPlayer -= EnemyLostPlayer;
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
        ScreenDamageMat.SetFloat(VIGNETTE_RADIUS, 0);
    }
    private void ScreenDamageEffect(float intensity)
    {
        if (ScreenDamageTask != null)
            StopCoroutine(ScreenDamageTask);
        ScreenDamageTask = StartCoroutine(ScreenDamage(intensity));
    }
    [ContextMenu("Test Screen Damage Effect")]
    void Test()
    {
        ScreenDamageEffect(0.5f);
        //ScreenDamageMat.SetFloat(VIGNETTE_RADIUS, 0.33f);
    }

    private IEnumerator ScreenDamage(float intensity)
    {
        ScreenDamageMat.SetColor(VIGNETTE_COLOR, DamageColor);
        float current = ScreenDamageMat.GetFloat(VIGNETTE_RADIUS);
        float t = 0f;
        while (Mathf.Abs(current - intensity) > 0.01f)
        {
            t += Time.deltaTime * VignetteSpeed;
            current = Mathf.Lerp(current, intensity, t);
            ScreenDamageMat.SetFloat(VIGNETTE_RADIUS, current);
            yield return null;
        }
        ScreenDamageMat.SetFloat(VIGNETTE_RADIUS, intensity);
        //var targetRadius = Remap(intensity, MinIntensity, MaxIntensity, MinVignetteRadius, MaxVignetteRadius);
        //var curRadius = 1f;
        //for (float t = 0; Mathf.Abs(curRadius - targetRadius) > 0.001f; t += Time.deltaTime * VignetteSpeed)
        //{
        //    curRadius = Mathf.Lerp(ScreenDamageMat.GetFloat(VIGNETTE_RADIUS), targetRadius, t);
        //    ScreenDamageMat.SetFloat(VIGNETTE_RADIUS, curRadius);
        //    yield return null;
        //}

        //curRadius = targetRadius;
        //ScreenDamageMat.SetFloat(VIGNETTE_RADIUS, curRadius);
        //for (float t = 0; Mathf.Abs(curRadius - 1f) > 0.001f; t += Time.deltaTime * VignetteSpeed)
        //{
        //    curRadius = Mathf.Lerp(targetRadius, 1, t);
        //    ScreenDamageMat.SetFloat(VIGNETTE_RADIUS, curRadius);
        //    yield return null;
        //}
        //curRadius = 1f;
        //ScreenDamageMat.SetFloat(VIGNETTE_RADIUS, curRadius);
    }

    private float Remap(float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        return Mathf.Lerp(toMin, toMax, Mathf.InverseLerp(fromMin, fromMax, value));
    }

    public static class SpecialEffects
    {
        public static void ScreenDamageEffect(float intensity) => Instance.ScreenDamageEffect(intensity);
        public static void ScreenStealthEffect(bool IsPlayerHidden)
        {
            if (Instance.ScreenDamageTask != null)
                Instance.StopCoroutine(Instance.ScreenDamageTask);
            if (IsPlayerHidden)
                Instance.ActivateHideVignette();
            else
                Instance.DeactivateHideVignette();
        }
        public static void ScreenDamageOverTimeEffect() => Instance.StartCoroutine(Instance.StartDamageOverTimeVignette());
        public static void StopScreenDamageOverTimeEffect() => Instance.StartCoroutine(Instance.StopDamageOverTimeVignette());
    }

    private IEnumerator StopDamageOverTimeVignette()
    {
        float current = ScreenDamageMat.GetFloat(VIGNETTE_RADIUS);
        float t = 0f;
        while (Mathf.Abs(current - 0) > 0.001f)
        {
            t += Time.deltaTime * VignetteSpeed;
            current = Mathf.Lerp(current, 0, t);
            ScreenDamageMat.SetFloat(VIGNETTE_RADIUS, current);
            yield return null;
        }
        ScreenDamageMat.SetFloat(VIGNETTE_RADIUS, 0);
    }

    private IEnumerator StartDamageOverTimeVignette()
    {
        var targetRadius = Remap(0.5f, -0f, 1, -0.95f, -0.901f);
        var curRadius = 1f;
        for (float t = 0; Mathf.Abs(curRadius - targetRadius) > 0.001f; t += Time.deltaTime * 0.05f)
        {
            curRadius = Mathf.Lerp(ScreenDamageMat.GetFloat(VIGNETTE_RADIUS), targetRadius, t);
            ScreenDamageMat.SetFloat(VIGNETTE_RADIUS, curRadius);
            Debug.Log("Damage effect started: " + curRadius);
            yield return null;
        }
    }

    private IEnumerator StartDetectionVignette()
    {
        var targetRadius = Remap(0.5f, -0f, 1, -0.35f, -0.001f);
        var curRadius = 1f;
        for (float t = 0; curRadius != targetRadius; t += Time.deltaTime - 0.01f)
        {
            curRadius = Mathf.Clamp(Mathf.Lerp(1, 0.1f, t), 1, 0.1f);
            Debug.Log("Detected effect");
            ScreenDamageMat.SetFloat(VIGNETTE_RADIUS, 0.35f);
            yield return null;
        }
    }

    public void SeenByEnemy(EnemyAI enemy)
    {
        if (Enemies.Contains(enemy)) return;
        Enemies.Add(enemy);
        if (Enemies.Count == 1)
        {
            ScreenDamageMat.SetFloat(VIGNETTE_RADIUS, 0.35f);

            //StartCoroutine(StartDetectionVignette());
        }
    }

    public void EnemyLostPlayer(EnemyAI enemy)
    {
        Enemies.Remove(enemy);
        if (Enemies.Count == 0)
        {
            ScreenDamageMat.SetFloat(VIGNETTE_RADIUS, 0);
        }
    }

    public void ActivateHideVignette()
    {
        ScreenDamageMat.SetColor(VIGNETTE_COLOR, StealthColor);
        //ScreenDamageMat.SetFloat(VIGNETTE_RADIUS, StealthMaxIntensity);
        if (ScreenDamageTask != null) StopCoroutine(ScreenDamageTask);
        ScreenDamageTask = StartCoroutine(FadeInHideVignette(StealthMaxIntensity));
    }

    public void DeactivateHideVignette()
    {
        ScreenDamageMat.SetColor(VIGNETTE_COLOR, StealthColor);
        if (ScreenDamageTask != null) StopCoroutine(ScreenDamageTask);
        ScreenDamageTask = StartCoroutine(FadeOutHideVignette());
    }

    private IEnumerator FadeInHideVignette(float intensity)
    {
        float current = ScreenDamageMat.GetFloat(VIGNETTE_RADIUS);
        float t = 0f;
        while (Mathf.Abs(current - intensity) > 0.01f)
        {
            t += Time.deltaTime * VignetteSpeed;
            current = Mathf.Lerp(current, intensity, t);
            ScreenDamageMat.SetFloat(VIGNETTE_RADIUS, current);
            yield return null;
        }
        ScreenDamageMat.SetFloat(VIGNETTE_RADIUS, intensity);
    }
    private IEnumerator FadeOutHideVignette()
    {
        float current = ScreenDamageMat.GetFloat(VIGNETTE_RADIUS);
        float t = 0f;
        while (Mathf.Abs(current - 0) > 0.001f)
        {
            t += Time.deltaTime * VignetteSpeed * 1.25f;
            current = Mathf.Lerp(current, 0, t);
            ScreenDamageMat.SetFloat(VIGNETTE_RADIUS, current);
            yield return null;
        }
        ScreenDamageMat.SetFloat(VIGNETTE_RADIUS, 0);
        ScreenDamageMat.SetColor(VIGNETTE_COLOR, DamageColor);
    }
}
