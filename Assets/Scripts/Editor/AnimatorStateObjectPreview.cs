using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

[CustomPreview(typeof(AnimatorState))]
public class AnimatorStateObjectPreview : ObjectPreview
{
    Editor Preview;
    int AnimationClipID;

    public override void Initialize(Object[] targets)
    {
        base.Initialize(targets);
        if (targets.Length > 1 || Application.isPlaying)
            return;
        AnimatorState state = (AnimatorState)target;
        if (state.motion && state.motion is AnimationClip clip)
        {
            Preview = Editor.CreateEditor(clip);
        }
    }

    public override void Cleanup()
    {
        base.Cleanup();
        CleanUpPreviewEditor();
    }

    public override bool HasPreviewGUI()
    {
        return Preview?.HasPreviewGUI() ?? false;
    }

    public override void OnInteractivePreviewGUI(Rect r, GUIStyle background)
    {
        base.OnInteractivePreviewGUI(r, background);
        AnimationClip currentClip = GetCurrentAnimationClip((AnimatorState)target);
        if(currentClip && currentClip.GetInstanceID() != AnimationClipID)
        {
            CleanUpPreviewEditor();
            Preview = Editor.CreateEditor(currentClip);
            AnimationClipID = currentClip.GetInstanceID();
            return;
        }
        if (Preview)
        {
            Preview.OnInteractivePreviewGUI(r, background);
        }
    }

    AnimationClip GetCurrentAnimationClip(AnimatorState state)
    {
        return state?.motion as AnimationClip;
    }

    void CleanUpPreviewEditor()
    {
        if (Preview)
        {
            Object.DestroyImmediate(Preview);
            Preview = null;
            AnimationClipID = 0;
        }
    }
}