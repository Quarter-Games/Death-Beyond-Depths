using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShaderSliderController : MonoBehaviour
{
    public Slider slider;
    public Material shaderMaterial;
    public string propertyName;

    // Start is called before the first frame update
    void Start()
    {
        UpdateShaderValue(slider.value);

        slider.onValueChanged.AddListener(UpdateShaderValue);
    }

    void OnDestroy()
    {
        if (slider != null)
        {
            slider.onValueChanged.RemoveListener(UpdateShaderValue);
        }
    }

    void UpdateShaderValue(float value)
    {
        if (shaderMaterial != null && !string.IsNullOrEmpty(propertyName))
        {
            shaderMaterial.SetFloat(propertyName, value);
        }
    }
}
