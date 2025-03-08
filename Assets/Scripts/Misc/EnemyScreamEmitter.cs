using UnityEngine;

public class EnemyScreamEmitter : MonoBehaviour
{
    [SerializeField] ParticleSystem scream;
    public void EmitScream()
    {
        scream.Play();
    }
}
