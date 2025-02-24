using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "SoundData", menuName = "Scriptable Objects/Sound Data")]
public class SoundData : ScriptableObject
{
    [SerializeField] float _soundTravelDistance;
    [SerializeField] AudioResource _soundClip;

    public float SoundTravelDistance => _soundTravelDistance;
    public AudioResource SoundClip => _soundClip;
}
