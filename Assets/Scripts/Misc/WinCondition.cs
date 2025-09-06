using System;
using System.Collections;
using UnityEngine;

public class WinCondition : MonoBehaviour
{
    [SerializeField] Canvas WinCanvas;
    [SerializeField] CaptainController player;
    private void OnEnable()
    {
    }
    private void OnDisable()
    {
        AudioManager.Instance.StartCoroutine(WaitForWin());
        player.enabled = false;
    }

    private IEnumerator WaitForWin()
    {
        yield return new WaitForSeconds(2.5f);
        WinCanvas.gameObject.SetActive(true);
    }
}
