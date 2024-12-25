using UnityEngine;

public class Room : MonoBehaviour
{
    PlayerCharacterController CachedPlayer;
    Enemy CachedEnemy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out CachedPlayer))
        {
            RoomManager.Instance.AssignPlayerToRoom(this, CachedPlayer);
        }
        else if (collision.TryGetComponent(out CachedEnemy))
        {
            RoomManager.Instance.AssignEnemyToRoom(this, CachedEnemy);
        }
    }
}
