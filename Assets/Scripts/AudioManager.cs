using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Références aux fichiers audio (.wav)
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

    private AudioSource audioSource;
    private AudioSource walkAudioSource; // AudioSource séparé pour la marche

    void Start()
    {
        // Ajouter un AudioSource au GameObject si non présent
        audioSource = gameObject.AddComponent<AudioSource>();

        // Ajouter un autre AudioSource pour les sons de marche
        walkAudioSource = gameObject.AddComponent<AudioSource>();

        // Exemple de musique d'ambiance à démarrer
        PlayCityAmbiant();
    }

    // Méthodes pour jouer les différents sons

    public void PlayCityAmbiant()
    {
        audioSource.clip = cityAmbiantSound;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlayJumpSound()
    {
        audioSource.PlayOneShot(jumpSound);
    }

    public void PlayTrapSound()
    {
        audioSource.PlayOneShot(trapSound);
    }

    public void PlayDeathSound()
    {
        audioSource.PlayOneShot(deathSound);
    }

    public void PlayDogSound()
    {
        audioSource.PlayOneShot(dogSound);
    }

    public void PlayEatSound()
    {
        audioSource.PlayOneShot(eatSound);
    }

    public void PlayExplosionSound()
    {
        audioSource.PlayOneShot(explosionSound);
    }

    public void PlayHitSound()
    {
        audioSource.PlayOneShot(hitSound);
    }

    public void PlayMenuSound()
    {
        audioSource.clip = menuSound;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlayTimerStressSound()
    {
        audioSource.clip = timerStressSound;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlayVictorySound()
    {
        audioSource.PlayOneShot(victorySound);
    }

    // Jouer et arrêter le son de marche

    public void PlayWalkSound()
    {
        if (!walkAudioSource.isPlaying)
        {
            walkAudioSource.clip = walkSound;
            walkAudioSource.loop = true;  // Faire boucler le son de marche
            walkAudioSource.Play();
        }
    }

    public void StopWalkSound()
    {
        if (walkAudioSource.isPlaying)
        {
            walkAudioSource.Stop(); // Arrêter le son de marche
        }
    }
}
