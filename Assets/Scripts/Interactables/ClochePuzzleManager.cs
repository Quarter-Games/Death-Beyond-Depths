using System.Collections.Generic;
using UnityEngine;

public class ClochePuzzleManager : MonoBehaviour
{
    [SerializeField] List<ClocheInteractable> Cloches;

    private void OnEnable()
    {
        if (Cloches == null || Cloches.Count == 0)
        {
            Debug.Log("No cloches assigned to ClochePuzzleManager.");
            return;
        }
        //DisableAllCloches();
        ClocheInteractable.OnClocheInteracted += DisableAllCloches;
    }

    private void OnDisable()
    {
        ClocheInteractable.OnClocheInteracted -= DisableAllCloches;
    }

    public void DisableAllCloches()
    {
        foreach (var cloche in Cloches)
        {
            cloche.DisableClocheInteractability();
            Debug.Log("Cloche disabled: " + cloche.name);
            Debug.Log(cloche.IsInteractable);
        }
    }

    public void EnableAllCloches()
    {
        foreach (var cloche in Cloches)
        {
            cloche.EnableClocheInteractability();
        }
    }
}
