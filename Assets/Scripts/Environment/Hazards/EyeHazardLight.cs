using UnityEngine;

public class EyeHazardLight : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerCharacterController player))
        {
            if (player.IsHidden)
            {
                return;
            }
            player.TakeDamage(999999);
        }
    }
}
