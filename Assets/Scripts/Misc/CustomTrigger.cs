using System;
using UnityEngine;

public class CustomTrigger : MonoBehaviour
{
    public event Action<Collider2D> EnterTrigger;
    public event Action<Collider2D> ExitTrigger;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnterTrigger?.Invoke(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ExitTrigger?.Invoke(collision);
    }
}
