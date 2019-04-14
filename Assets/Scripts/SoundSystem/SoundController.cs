using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField]
    SoundLibrary soundLibrary;
    public SoundLibrary SoundLibrary { get { return soundLibrary; } }

    [SerializeField]
    AudioSource audioSource;

    public void PlaySound(Sounds soundName)
    {
        Sound sound = soundLibrary.Data.Find(item => item.name == soundName);
        audioSource.volume = sound.volume;
        audioSource.PlayOneShot(sound.audioClip);
    }
}