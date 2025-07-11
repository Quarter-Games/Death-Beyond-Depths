using MoreMountains.Feedbacks;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    [SerializeField] MMF_Player MMF_Player;

    private void OnValidate()
    {
        if (MMF_Player == null)
        {
            MMF_Player = GetComponent<MMF_Player>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            MMF_Player.ResumeFeedbacks();
        }
    }
}
