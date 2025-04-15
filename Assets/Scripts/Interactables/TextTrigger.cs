using UnityEngine;

public class TextTrigger : MonoBehaviour
{
    [SerializeField] TriggerType TriggerType;
    [SerializeField, TextArea] string Text;
    [SerializeField] Color TextColor;

    PlayerCharacterController Player;
    bool CanBeTriggered = false;
    bool HasTextBeenTriggered = false;

    private void OnEnable()
    {
        if (TriggerType == TriggerType.OnInteract)
        {
            PlayerCharacterController.OnInteract += TriggerText;
        }
    }

    private void OnDisable()
    {
        PlayerCharacterController.OnInteract -= TriggerText;
    }

    private void Start()
    {
        Player = FindFirstObjectByType<PlayerCharacterController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerCharacterController>() == null) return;
        CanBeTriggered = true;
        if (TriggerType == TriggerType.OnEnterOnce && !HasTextBeenTriggered)
        {
            TriggerText();
            HasTextBeenTriggered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerCharacterController>() == null) return;
        CanBeTriggered = false;
    }

    private void TriggerText()
    {
        if (!CanBeTriggered) return;
        Player.CreatePlayerText(Text, TextColor);
    }
}

enum TriggerType
{
    OnEnterOnce,
    OnInteract
}