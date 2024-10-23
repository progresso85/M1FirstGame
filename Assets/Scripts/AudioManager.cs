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

    void Start()
    {
        
        audioSource = GetComponent<AudioSource>();

        
        PlayBackgroundMusic();
    }

    
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

     
    public void PlayWalkSound()
    {
        audioSource.clip = walkSound;
        audioSource.loop = true;  
        audioSource.Play();
    }

}