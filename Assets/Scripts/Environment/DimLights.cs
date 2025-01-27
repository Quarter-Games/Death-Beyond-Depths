using UnityEngine;

public class PointLightDimmer : MonoBehaviour
{
    // The minimum and maximum intensity values
    public float minIntensity = 0.8f;
    public float maxIntensity = 1.2f;

    // The speed of the dimming effect
    public float flickerSpeed = 1.0f;

    // Original intensity of the light
    private float baseIntensity;

    // Reference to the Light component
    private Light pointLight;

    private void Start()
    {
        // Get the Light component attached to the GameObject
        pointLight = GetComponent<Light>();

        // Save the base intensity
        if (pointLight != null)
        {
            baseIntensity = pointLight.intensity;
        }
        else
        {
            Debug.LogError("No Light component found on this GameObject.");
        }
    }

    private void Update()
    {
        if (pointLight != null)
        {
            // Calculate the new intensity using Perlin noise for smooth variation
            float noise = Mathf.PerlinNoise(Time.time * flickerSpeed, 0);
            float newIntensity = Mathf.Lerp(minIntensity, maxIntensity, noise);

            // Apply the new intensity while keeping the original intensity as a base
            pointLight.intensity = baseIntensity * newIntensity;
        }
    }
}
