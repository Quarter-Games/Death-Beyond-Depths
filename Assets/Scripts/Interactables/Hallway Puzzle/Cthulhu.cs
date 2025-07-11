using System;
using UnityEngine;

public class Cthulhu : MonoBehaviour
{
    [SerializeField] Renderer LeftEye;
    [SerializeField] Renderer RightEye;
    [SerializeField] Material DisabledEye;
    [SerializeField] Material EnabledEye;
    private void Update()
    {
        transform.LookAt(Camera.main.transform);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }
    public void Init(int iteration)
    {

        if (iteration == 0) NoColors();
        else BothEyes();
        if (iteration < 3) transform.localScale *= 1.5f;
    }

    private void OneEye()
    {
        LeftEye.material = EnabledEye;
        RightEye.material = DisabledEye;
    }

    private void BothEyes()
    {
        LeftEye.material = EnabledEye;
        RightEye.material = EnabledEye;
    }

    private void NoColors()
    {
        LeftEye.material = DisabledEye;
        RightEye.material = DisabledEye;
    }
}
