using MoreMountains.Feedbacks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class DialogueController : MonoBehaviour
{
    public static event Action OnDialogueEnd;
    [SerializeField] MMF_Player IntroMMFPlayer;
    [SerializeField] MMF_Player MMFPlayer;
    [SerializeField] MMF_Player OutroMMFPlayer;

    [SerializeField] TMPro.TMP_Text MariaDialogueText;
    [SerializeField] TMPro.TMP_Text CaptainDialogueText;
    [SerializeField] GameObject MariaDialogueBox;
    [SerializeField] GameObject CaptainDialogueBox;
    [SerializeField] List<DialogueRow> dialogueRows;

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
        yield return IntroMMFPlayer.PlayFeedbacksCoroutine(Vector3.one);
        var setActiveFeedback = MMFPlayer.GetFeedbackOfType<MMF_SetActive>();
        var revealFeedback = MMFPlayer.GetFeedbackOfType<MMF_TMPTextReveal>();
        var soundFeedback = MMFPlayer.GetFeedbackOfType<MMF_Sound>();
        for (int i = 0; i < dialogueRows.Count; i++)
        {
            var row = dialogueRows[i];
            if (row.isMaria)
            {
                CaptainDialogueBox.SetActive(false);
                setActiveFeedback.TargetGameObject = MariaDialogueBox;
                revealFeedback.TargetTMPText = MariaDialogueText;
            }
            else
            {
                MariaDialogueBox.SetActive(false);
                setActiveFeedback.TargetGameObject = CaptainDialogueBox;
                revealFeedback.TargetTMPText = CaptainDialogueText;
            }
            revealFeedback.NewText = row.text;
            if (soundFeedback != null)
            {
                soundFeedback.Sfx = row.audioSequence;
            }
            yield return MMFPlayer.PlayFeedbacksCoroutine(Vector3.one);
        }
        yield return OutroMMFPlayer.PlayFeedbacksCoroutine(Vector3.one);
        CaptainDialogueBox.SetActive(false);
        MariaDialogueBox.SetActive(false);
        OnDialogueEnd?.Invoke();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MMFPlayer.StopFeedbacks();
        }
    }
    [Serializable]
    class DialogueRow
    {
        public bool isMaria;
        [Multiline] public string text;
        public AudioClip audioSequence;
    }
}
