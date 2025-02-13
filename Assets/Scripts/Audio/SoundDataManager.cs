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

    public void EmitSound(Transform origin, float radius = 5f)
    {
        lastOrigin = origin;
        lastRadius = radius;
        raycastResults.Clear();
        List<Transform> affectedEnemies = new List<Transform>();

        for (int i = 0; i < rayCount; i++)
        {
            Vector2 randomDir = Random.insideUnitCircle.normalized;
            Vector2 originPos = origin.position;
            RaycastHit2D hit = Physics2D.Raycast(originPos, randomDir, radius, obstacleLayer | enemyLayer);
            if (hit.collider != null)
            {
                if ((obstacleLayer.value & (1 << hit.collider.gameObject.layer)) != 0)
                {
                    raycastResults.Add(new RaycastInfo(originPos, hit.point, Color.red));
                    Debug.Log(hit.collider.gameObject);
                    continue;
                }
                if ((enemyLayer.value & (1 << hit.collider.gameObject.layer)) != 0)
                {
                    affectedEnemies.Add(hit.transform);
                    raycastResults.Add(new RaycastInfo(originPos, hit.point, Color.green));
                }
                else
                {
                    raycastResults.Add(new RaycastInfo(originPos, hit.point, Color.yellow));
                }
            }
            else
            {
                raycastResults.Add(new RaycastInfo(originPos, originPos + randomDir * radius, Color.yellow));
            }
        }

        foreach (Transform enemy in affectedEnemies)
        {
            enemy.GetComponent<IHearing>()?.OnHeardSound(origin.position);
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
