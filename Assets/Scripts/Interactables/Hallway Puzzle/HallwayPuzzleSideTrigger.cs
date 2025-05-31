using System;
using UnityEngine;

public class HallwayPuzzleSideTrigger : MonoBehaviour
{
    public static event Action<HallwayPuzzleSideTrigger> OnSideTriggerEntered;
    public bool isRight;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnSideTriggerEntered?.Invoke(this);
        }
    }
}
