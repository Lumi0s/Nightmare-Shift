using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private List<AudioClip> musicClips;

    // DŸwiêki bohatera
    [SerializeField] private AudioClip basicAttackSound;
    [SerializeField] private AudioClip fireballAttackSound;
    [SerializeField] private AudioClip lightningAttackSound;
    [SerializeField] private AudioClip playerHitSound;
    [SerializeField] private AudioClip playerDrinkSound;
    [SerializeField] private AudioClip playerDefenseSound;

    // DŸwiêki przeciwnika
    [SerializeField] private AudioClip enemyAttackSound;
    [SerializeField] private AudioClip enemyHitSound;

    [SerializeField] private float fadeDuration = 1.5f;

    private int currentClipIndex = 0;
    private bool isTransitioning = false;

    private void Start()
    {
        if (musicClips.Count > 0)
        {
            PlayClip(currentClipIndex);
        }
        else
        {
            Debug.LogWarning("Lista klipów muzycznych jest pusta.");
        }
    }

    private void Update()
    {
        if (!musicSource.isPlaying && musicSource.clip != null)
        {
            StartCoroutine(FadeOutAndPlayNext());
        }
    }

    private void PlayClip(int index)
    {
        if (index < musicClips.Count)
        {
            musicSource.clip = musicClips[index];
            musicSource.Play();
            StartCoroutine(FadeIn());
            Debug.Log($"Odtwarzanie klipu: {musicClips[index].name}");
        }
        else
        {
            Debug.LogWarning("Indeks klipu wykracza poza listê.");
        }
    }

    private IEnumerator FadeOutAndPlayNext()
    {
        isTransitioning = true;

        // Fade-out
        float startVolume = musicSource.volume;
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            musicSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }
        musicSource.volume = 0;
        musicSource.Stop();

        // Ustawienie na nastêpny klip
        currentClipIndex = (currentClipIndex + 1) % musicClips.Count;
        PlayClip(currentClipIndex);

        isTransitioning = false;
    }

    private IEnumerator FadeIn()
    {
        // Fade-in
        float targetVolume = 0.1f;
        musicSource.volume = 0;
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            musicSource.volume = Mathf.Lerp(0, targetVolume, t / fadeDuration);
            yield return null;
        }
        musicSource.volume = targetVolume;
    }

    private IEnumerator PlaySoundWithFadeOut(AudioClip clip, float fadeOutDuration)
    {
        sfxSource.PlayOneShot(clip);
        float startVolume = sfxSource.volume;

        // Czekanie a¿ dŸwiêk siê rozpocznie, zanim zaczniemy go œciszaæ
        yield return new WaitForSeconds(clip.length - fadeOutDuration);

        // Stopniowe œciszanie dŸwiêku
        for (float t = 0; t < fadeOutDuration; t += Time.deltaTime)
        {
            sfxSource.volume = Mathf.Lerp(startVolume, 0, t / fadeOutDuration);
            yield return null;
        }

        sfxSource.volume = 0.7f;
    }

    public void PlayBasicAttackSound()
    {
        if (basicAttackSound != null)
        {
            sfxSource.PlayOneShot(basicAttackSound);
        }
        else
        {
            Debug.LogWarning("Brak przypisanego dŸwiêku podstawowego ataku.");
        }
    }

    public void PlayFireballAttackSound()
    {
        if (fireballAttackSound != null)
        {
            StartCoroutine(PlaySoundWithFadeOut(fireballAttackSound, 1.0f));
        }
        else
        {
            Debug.LogWarning("Brak przypisanego dŸwiêku ataku kul¹ ognia.");
        }
    }

    public void PlayLightningAttackSound()
    {
        if (lightningAttackSound != null)
        {
            sfxSource.PlayOneShot(lightningAttackSound);
        }
        else
        {
            Debug.LogWarning("Brak przypisanego dŸwiêku ataku b³yskawic¹.");
        }
    }

    public void PlayPlayerHitSound()
    {
        if (playerHitSound != null)
        {
            sfxSource.PlayOneShot(playerHitSound);
        }
        else
        {
            Debug.LogWarning("Brak przypisanego dŸwiêku obra¿eñ.");
        }
    }

    public void PlayPlayerDrinkSound()
    {
        if (playerDrinkSound != null)
        {
            sfxSource.PlayOneShot(playerDrinkSound);
        }
        else
        {
            Debug.LogWarning("Brak przypisanego dŸwiêku obra¿eñ.");
        }
    }
    public void PlayPlayerDefenseSound()
    {
        if (playerDefenseSound != null)
        {
            sfxSource.PlayOneShot(playerDefenseSound);
        }
        else
        {
            Debug.LogWarning("Brak przypisanego dŸwiêku obra¿eñ.");
        }
    }

    public void PlayEnemyAttackSound()
    {
        if (enemyAttackSound != null)
        {
            StartCoroutine(PlaySoundWithDelay(enemyAttackSound, 1.0f));
        }
        else
        {
            Debug.LogWarning("Brak przypisanego dŸwiêku ataku przeciwnika.");
        }
    }

    public void PlayEnemyHitSound()
    {
        if (enemyHitSound != null)
        {
            sfxSource.PlayOneShot(enemyHitSound);
        }
        else
        {
            Debug.LogWarning("Brak przypisanego dŸwiêku obra¿eñ przeciwnika.");
        }
    }

    private IEnumerator PlaySoundWithDelay(AudioClip clip, float delay)
    {
        yield return new WaitForSeconds(delay);
        sfxSource.PlayOneShot(clip);
    }
}
