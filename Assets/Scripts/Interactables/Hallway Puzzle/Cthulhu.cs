using System;
using UnityEngine;

public class Cthulhu : MonoBehaviour
{
    [SerializeField] Renderer LeftEye;
    [SerializeField] Renderer RightEye;
    [SerializeField] Material DisabledEye;
    [SerializeField] Material EnabledEye;
    private void Awake()
    {
    }
    public void Init(int iteration)
    {
        switch (iteration % 3)
        {
            case 0: NoColors(); break;
            case 1: BothEyes(); break;
            case 2: OneEye(); break;
        }
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
