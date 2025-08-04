using MoreMountains.Feedbacks;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    [SerializeField] MMF_Player MMFPlayer;

    private void OnValidate()
    {
        if (MMFPlayer == null)
        {
            MMFPlayer = GetComponent<MMF_Player>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            MMFPlayer.ResumeFeedbacks();
        }
    }
}
