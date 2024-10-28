using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Références aux fichiers audio
    public AudioClip cityAmbiantSound;
    public AudioClip jumpSound;
    public AudioClip trapSound;
    public AudioClip deathSound;
    public AudioClip dogSound;
    public AudioClip eatSound;
    public AudioClip explosionSound;
    public AudioClip hitSound;
    public AudioClip menuSound;
    public AudioClip timerStressSound;
    public AudioClip victorySound;
    public AudioClip walkSound;

    // Variables de volume
    [Range(0f, 1f)] public float cityAmbiantVolume = 0.5f;
    [Range(0f, 1f)] public float jumpVolume = 0.5f;
    [Range(0f, 1f)] public float trapVolume = 0.5f;
    [Range(0f, 1f)] public float deathVolume = 0.5f;
    [Range(0f, 1f)] public float dogVolume = 0.5f;
    [Range(0f, 1f)] public float eatVolume = 0.5f;
    [Range(0f, 1f)] public float explosionVolume = 0.5f;
    [Range(0f, 1f)] public float hitVolume = 0.5f;
    [Range(0f, 1f)] public float menuVolume = 0.5f;
    [Range(0f, 1f)] public float timerStressVolume = 0.5f;
    [Range(0f, 1f)] public float victoryVolume = 0.5f;
    [Range(0f, 1f)] public float walkVolume = 0.5f;

    private AudioSource audioSource;
    private AudioSource walkAudioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        walkAudioSource = gameObject.AddComponent<AudioSource>();

        PlayCityAmbiant();
    }

    // Méthodes pour jouer les sons avec le volume défini
    public void PlayCityAmbiant()
    {
        audioSource.clip = cityAmbiantSound;
        audioSource.volume = cityAmbiantVolume;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlayJumpSound() => audioSource.PlayOneShot(jumpSound, jumpVolume);
    public void PlayTrapSound() => audioSource.PlayOneShot(trapSound, trapVolume);
    public void PlayDeathSound() => audioSource.PlayOneShot(deathSound, deathVolume);
    public void PlayDogSound() => audioSource.PlayOneShot(dogSound, dogVolume);
    public void PlayEatSound() => audioSource.PlayOneShot(eatSound, eatVolume);
    public void PlayExplosionSound() => audioSource.PlayOneShot(explosionSound, explosionVolume);
    public void PlayHitSound() => audioSource.PlayOneShot(hitSound, hitVolume);
    public void PlayMenuSound() => PlayLoop(menuSound, menuVolume);
    public void PlayTimerStressSound() => PlayLoop(timerStressSound, timerStressVolume);
    public void PlayVictorySound() => audioSource.PlayOneShot(victorySound, victoryVolume);

    // Jouer et arrêter le son de marche
    public void PlayWalkSound()
    {
        if (!walkAudioSource.isPlaying)
        {
            walkAudioSource.clip = walkSound;
            walkAudioSource.volume = walkVolume;
            walkAudioSource.loop = true;
            walkAudioSource.Play();
        }
    }

    public void StopWalkSound()
    {
        if (walkAudioSource.isPlaying)
        {
            walkAudioSource.Stop();
        }
    }

    public bool IsWalkSoundPlaying() => walkAudioSource.isPlaying;

    // Méthodes pour changer dynamiquement le volume de chaque son
    public void SetCityAmbiantVolume(float volume) => SetVolume(cityAmbiantSound, ref cityAmbiantVolume, volume);
    public void SetWalkVolume(float volume) => SetVolume(walkSound, ref walkVolume, volume);

    public void SetJumpVolume(float volume) => jumpVolume = volume;
    public void SetTrapVolume(float volume) => trapVolume = volume;
    public void SetDeathVolume(float volume) => deathVolume = volume;
    public void SetDogVolume(float volume) => dogVolume = volume;
    public void SetEatVolume(float volume) => eatVolume = volume;
    public void SetExplosionVolume(float volume) => explosionVolume = volume;
    public void SetHitVolume(float volume) => hitVolume = volume;
    public void SetMenuVolume(float volume) => menuVolume = volume;
    public void SetTimerStressVolume(float volume) => timerStressVolume = volume;
    public void SetVictoryVolume(float volume) => victoryVolume = volume;

    private void SetVolume(AudioClip clip, ref float volumeVar, float volume)
    {
        volumeVar = volume;
        if (audioSource.clip == clip)
        {
            audioSource.volume = volume;
        }
    }

    private void PlayLoop(AudioClip clip, float volume)
    {
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.loop = true;
        audioSource.Play();
    }
}

