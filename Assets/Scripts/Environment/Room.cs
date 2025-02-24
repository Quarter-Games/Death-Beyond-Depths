using System;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] SpriteRenderer BlackSpriteSquare;

    PlayerCharacterController CachedPlayer;
    Enemy CachedEnemy;

    private void OnValidate()
    {
        if (BlackSpriteSquare == null)
        {
            BlackSpriteSquare = GetComponent<SpriteRenderer>();
        }
    }
    private void Start()
    {
        //BlackSpriteSquare.bounds = GetComponent<Collider2D>().bounds;
        //BlackSpriteSquare.size = GetComponent<Collider2D>().bounds.size;
        //BlackSpriteSquare.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out CachedPlayer))
        {
            LightenRoom();
            RoomManager.Instance.AssignPlayerToRoom(this, CachedPlayer);
        }
        else if (collision.TryGetComponent(out CachedEnemy))
        {
            RoomManager.Instance.AssignEnemyToRoom(this, CachedEnemy);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out CachedPlayer))
        {
            DarkenRoom();
        }
    }

    [ContextMenu("Darken Room")]
    private void DarkenRoom()
    {
        //TODO add DoTween
        //BlackSpriteSquare.enabled = true;
    }

    private void LightenRoom()
    {
        BlackSpriteSquare.enabled = false;
    }
}
