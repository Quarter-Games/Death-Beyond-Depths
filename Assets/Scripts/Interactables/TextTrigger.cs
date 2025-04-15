using Unity.Cinemachine;
using UnityEditor;
using UnityEngine;

public class TextTrigger : MonoBehaviour
{
    [SerializeField] TriggerType TriggerType;
    [SerializeField, TextArea] string Text;
    [SerializeField] Color TextColor;
    public bool OverrideTextDuration = false;
    [HideInInspector] public float TextDuration = 3f;

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
        if(OverrideTextDuration)
        {
            Player.CreatePlayerText(Text, TextColor, TextDuration);
            return;
        }
        Player.CreatePlayerText(Text, TextColor);
    }
}

[CustomEditor(typeof(TextTrigger))]
public class TextTriggerEditor : Editor
{
    TextTrigger textTrigger;
    private void OnEnable()
    {
        textTrigger = (TextTrigger)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (textTrigger.OverrideTextDuration)
        {
            textTrigger.TextDuration = EditorGUILayout.FloatField("Text Duration", textTrigger.TextDuration);
        }
        if (GUI.changed)
        {
            EditorUtility.SetDirty(textTrigger);
        }
    }
}

enum TriggerType
{
    OnEnterOnce,
    OnInteract
}