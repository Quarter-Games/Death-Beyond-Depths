using UnityEngine;

[CreateAssetMenu(fileName = "SoundData", menuName = "Scriptable Objects/Sound Data")]
public class SoundData : ScriptableObject
{
    [SerializeField] float _soundTravelDistance;
    [SerializeField] AudioClip _soundClip;

    public float SoundTravelDistance => _soundTravelDistance;
    public AudioClip SoundClip => _soundClip;
}
