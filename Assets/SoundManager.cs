using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip nomalClickSound;
    public AudioClip errorClickSound;
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
        // Debug.Log("Playing win sound");
        // audioSource.clip = winSound;
        // audioSource.Play();
    }

    public void PlayLoseSound()
    {
        lossSource.Play();
        // Debug.Log("Playing lose sound");
        // audioSource.clip = loseSound;
        // audioSource.Play();
    }

    public void PlayNomalClickSound ()
    {
        audioSource.clip= nomalClickSound;
        audioSource.Play();
    }

    public void PlayErrorClickSound ()
    {
        audioSource.clip= errorClickSound;
        audioSource.Play();
    }

    public void PlayGrowUpSound()
    {
        growupSource.Play();
    }
}