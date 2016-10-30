using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField]
    private AudioSource _shiftAudio;
    [SerializeField]
    private AudioSource _jumpAudio;
    [SerializeField]
    private AudioSource _levelClearedAudio;
    [SerializeField]
    private AudioSource _failedAudio;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            if (Instance != this)
            {
                DestroyImmediate(gameObject);
            }
        }
    }

    private void PlayAudioSource(AudioSource source)
    {
        if (source != null)
        {
            if (source.isPlaying)
            {
                source.Stop();
            }
            source.Play();
        }
    }

    private void StopAudioSource(AudioSource source)
    {
        if (source != null)
        {
            if (source.isPlaying)
            {
                source.Stop();
            }
        }
    }

    public void PlayShiftSound()
    {
        PlayAudioSource(_shiftAudio);
    }

    public void PlayJumpSound()
    {
        PlayAudioSource(_jumpAudio);
    }

    public void PlayLevelClearSound()
    {
        PlayAudioSource(_levelClearedAudio);
    }

    public void PlayFailedSound()
    {
        PlayAudioSource(_failedAudio);
    }
}
