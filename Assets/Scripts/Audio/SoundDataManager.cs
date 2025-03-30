using System.Collections.Generic;
using UnityEngine;

public class SoundDataManager : MonoBehaviour
{
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private int rayCount = 100;

    private List<RaycastInfo> raycastResults = new List<RaycastInfo>();
    private Transform lastOrigin;
    private float lastRadius;

    public static SoundDataManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public void EmitSound(Transform origin, float baseRadius = 5f)
    {
        lastOrigin = origin;
        lastRadius = baseRadius;
        raycastResults.Clear();
        List<EnemyAI> affectedEnemies = new List<EnemyAI>();
        for (int i = 0; i < rayCount; i++)
        {
            Vector2 randomDir = Random.insideUnitCircle.normalized;
            Vector2 originPos = origin.position;
            RaycastHit2D hit = Physics2D.Raycast(originPos, randomDir, baseRadius, obstacleLayer | enemyLayer);
            Vector2 rayEndPoint = originPos + randomDir * baseRadius;
            if (hit.collider != null)
            {
                if ((obstacleLayer.value & (1 << hit.collider.gameObject.layer)) != 0)
                {
                    raycastResults.Add(new RaycastInfo(originPos, hit.point, Color.red));
                    continue;
                }
                if ((enemyLayer.value & (1 << hit.collider.gameObject.layer)) != 0)
                {
                    EnemyAI enemy = hit.collider.GetComponent<EnemyAI>();
                    if (enemy != null)
                    {
                        affectedEnemies.Add(enemy);
                        raycastResults.Add(new RaycastInfo(originPos, hit.point, Color.green));
                    }
                }
                rayEndPoint = hit.point;
            }
            else
            {
                raycastResults.Add(new RaycastInfo(originPos, rayEndPoint, Color.yellow));
            }
            foreach (EnemyAI enemy in EnemyAI.Enemies)
            {
                if (enemy == null) continue;
                if (Vector2.Distance(rayEndPoint, enemy.transform.position) <= enemy.SoundRadius)
                {
                    if (affectedEnemies.Contains(enemy)) continue;
                    affectedEnemies.Add(enemy);
                }
            }
        }
        foreach (EnemyAI enemy in affectedEnemies)
        {
            enemy.OnHeardSound(origin.position);
        }
    }


    private void OnDrawGizmos()
    {
        if (lastOrigin == null) return;
        foreach (var rayInfo in raycastResults)
        {
            Gizmos.color = rayInfo.color;
            Gizmos.DrawLine(rayInfo.start, rayInfo.end);
            Gizmos.DrawSphere(rayInfo.end, 0.05f);
        }
        Gizmos.color = new Color(1, 1, 0, 0.2f);
        Gizmos.DrawWireSphere(lastOrigin.position, lastRadius);
    }

    private class RaycastInfo
    {
        public Vector2 start;
        public Vector2 end;
        public Color color;
        public RaycastInfo(Vector2 start, Vector2 end, Color color)
        {
            this.start = start;
            this.end = end;
            this.color = color;
        }
    }
}
