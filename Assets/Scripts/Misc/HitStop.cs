using System.Collections;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    static bool IsTimeSlowed = false;

    public static IEnumerator TimeSlow(float power, float timeInSeconds)
    {
        if (IsTimeSlowed || power <= 0) yield return null;
        Debug.Log("Time slow!");
        Time.timeScale = 1 / power;
        IsTimeSlowed= true;
        yield return new WaitForSecondsRealtime(timeInSeconds);
        Time.timeScale = 1;
        IsTimeSlowed = false;
    }
}
