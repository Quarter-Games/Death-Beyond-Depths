using MoreMountains.Feedbacks;
using System;
using System.Collections;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    public static event Action OnDialogueEnd;
    [SerializeField] MMF_Player MMFPlayer;

    private void OnValidate()
    {
        if (MMFPlayer == null)
        {
            MMFPlayer = GetComponent<MMF_Player>();
        }
    }
    private void OnEnable()
    {
        StartCoroutine(StartDialogue());
    }
    IEnumerator StartDialogue()
    {
        yield return MMFPlayer.PlayFeedbacksCoroutine(Vector3.one);
        OnDialogueEnd?.Invoke();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MMFPlayer.ResumeFeedbacks();
        }
    }
}
