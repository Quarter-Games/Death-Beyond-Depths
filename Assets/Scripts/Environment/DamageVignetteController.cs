using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageVignetteController : MonoBehaviour
{
    [SerializeField] Material ScreenDamageMat;
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
        var targetRadius = Remap(intensity, -1, 1, -0.75f, -0.001f);
        var curRadius = 1f;
        for (float t = 0; curRadius != targetRadius; t += Time.deltaTime - 0.01f)
        {
            curRadius = Mathf.Clamp(Mathf.Lerp(1, targetRadius, t), 1, targetRadius);
            ScreenDamageMat.SetFloat("_Vignette_Radius", curRadius);
            yield return null;
        }
        for (float t = 0; curRadius < 1; t += Time.deltaTime - 0.001f)
        {
            curRadius = Mathf.Lerp(targetRadius, 1, t);
            ScreenDamageMat.SetFloat("_Vignette_Radius", curRadius);
            yield return null;
        }
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
