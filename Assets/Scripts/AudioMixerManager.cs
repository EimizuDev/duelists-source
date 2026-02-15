using UnityEngine;
using UnityEngine.Audio;

public class AudioMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;

    public void SetVolume(float level)
    {
        _audioMixer.SetFloat("masterVolume", Mathf.Log10(level) * 20f);
    }
}
