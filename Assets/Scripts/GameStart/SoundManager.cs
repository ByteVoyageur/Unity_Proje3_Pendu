using UnityEngine;


namespace Pendu.GameStart{
    /// <summary>
    /// Manages sound effects for the game.
    /// Provides methods to play fifferent game sounds such as winning, losing, clicks, and more.
    /// </summary>
public class SoundManager : MonoBehaviour
{
    public AudioClip errorClickSound;
    public AudioSource normalClickSound;
    public AudioSource victoirSource;
    public AudioSource lossSource;
    public AudioSource growupSource;
    public AudioSource soundBingo;

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

    public void PlaySoundBingo ()
    {
        soundBingo.Play();
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
}
