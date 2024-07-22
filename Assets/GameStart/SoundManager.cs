using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip errorClickSound;
    public AudioSource normalClickSound;
    public AudioSource victoirSource;
    public AudioSource lossSource;
    public AudioSource growupSource;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayWinSound()
    {
        victoirSource.Play();
    }

    public void PlayLoseSound()
    {
        lossSource.Play();
    }

    public void PlayNormalClickSound()
    {
        normalClickSound.Play();
    }

    public void PlayErrorClickSound()
    {
        audioSource.clip = errorClickSound;
        audioSource.Play();
    }

    public void PlayGrowUpSound()
    {
        growupSource.Play();
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
        normalClickSound.volume = volume;
        victoirSource.volume = volume;
        lossSource.volume = volume;
        growupSource.volume = volume;
    }
}