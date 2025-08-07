using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioClip flipClip;
    public AudioClip matchClip;
    public AudioClip mismatchClip;
    public AudioClip gameOverClip;

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlayFlip() => audioSource.PlayOneShot(flipClip);
    public void PlayMatch() => audioSource.PlayOneShot(matchClip);
    public void PlayMismatch() => audioSource.PlayOneShot(mismatchClip);
    public void PlayGameOver() => audioSource.PlayOneShot(gameOverClip);
}
