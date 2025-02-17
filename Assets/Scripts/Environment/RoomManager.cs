using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class RoomManager : MonoBehaviour
{
    [SerializeField] List<GameObject> RoomList;
    [SerializeField] Volume PostProcessingEffects;
    [SerializeField] float BlurDuration;

    private bool IsPlayerSpawning = false;

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
        if (IsPlayerSpawning)
        {
            StartCoroutine(ApplyBlurEffect());
        }
        else
        {
            IsPlayerSpawning = true;
        }
    }

    private IEnumerator ApplyBlurEffect()
    {
        if (PostProcessingEffects == null) return;
        PostProcessingEffects.weight = 1;
        yield return new WaitForSeconds(BlurDuration);
        PostProcessingEffects.weight = 0;
    }
}
