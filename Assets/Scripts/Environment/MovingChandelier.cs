using UnityEngine;

public class ChandelierSwing : MonoBehaviour
{
    public float swingSpeed = 1.0f; // Speed of the swing
    public float swingAngle = 15.0f; // Maximum angle of swing

    private float startTime;

    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        float angle = swingAngle * Mathf.Sin((Time.time - startTime) * swingSpeed);
        transform.localRotation = Quaternion.Euler(angle, 0, 0);
    }
}
