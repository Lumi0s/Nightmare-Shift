using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SoundEffect
{
    public string soundName;
    public AudioClip clip;
    [Range(0f,1f)]public float volume = 1f; 
    public bool loop;
    [HideInInspector] public AudioSource audioSource;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private List<SoundEffect> soundEffectList = new List<SoundEffect>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeSoundEffects();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeSoundEffects()
    {
        foreach (var soundEffect in soundEffectList)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = soundEffect.clip;
            audioSource.volume = soundEffect.volume; // Set the volume of the AudioSource
            soundEffect.audioSource = audioSource;
            audioSource.loop = soundEffect.loop;
        }
    }

    public void PlaySound(string soundName)
    {
        SoundEffect soundEffect = soundEffectList.Find(se => se.soundName == soundName);
        if (soundEffect != null && soundEffect.audioSource != null)
        {
            soundEffect.audioSource.Play();
        }
    }

    public void StopSound(string soundName)
    {
        SoundEffect soundEffect = soundEffectList.Find(se => se.soundName == soundName);
        if (soundEffect != null && soundEffect.audioSource != null)
        {
            soundEffect.audioSource.Stop();
        }
    }
}