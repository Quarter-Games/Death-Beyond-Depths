using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] List<List<GameObject>> RoomList;

    public static RoomManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public void AssignEnemyToRoom(Room room, Enemy enemy)
    {
        enemy.CurrentRoom = room;
    }

    public void AssignPlayerToRoom(Room room, PlayerCharacterController Player)
    {
        Player.CurrentRoom = room;
    }
}
