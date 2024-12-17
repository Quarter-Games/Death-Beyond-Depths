using UnityEngine;

public class Enemy : Character
{
    [Range(0f, 360f), SerializeField] public float Angle = 45f;
    [SerializeField] public float Radius = 5f;

    public void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 1f, 0f, 0.3f);
        DrawVisionCone();
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        DrawVisionCone();
    }

    public void DrawVisionCone()
    {
        UnityEditor.Handles.color = new Color(1f, 1f, 0f, 0.1f);
        Vector3 startDirection = Quaternion.Euler(0, 0, -Angle / 2) * Vector3.right;
        UnityEditor.Handles.DrawSolidArc(transform.position, Vector3.forward, startDirection, Angle, Radius);
    }

    public void TakeDamage(int damage)
    {
        stats.TakeDamage(damage);
        Debug.Log("Attacked for " + damage + " damage");
    }
}
