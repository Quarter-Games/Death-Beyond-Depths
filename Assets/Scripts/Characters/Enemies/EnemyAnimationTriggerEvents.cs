using UnityEngine;

public class EnemyAnimationTriggerEvents : MonoBehaviour
{
    EnemyAI Enemy;

    private void Start()
    {
        transform.parent.TryGetComponent(out Enemy);
    }

    public void TriggerCurrentStateAnimationEvent()
    {
        if (Enemy != null)
        {
            Enemy.TriggerCurrentAnimationEvent();
        }
    }
}
