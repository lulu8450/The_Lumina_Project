using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public AudioClip jumpSound;
    public AudioClip shootSound;
    public AudioClip damageSound;
    public AudioSource audioSource;

    void Awake()
    {
        // Get the AudioSource component.
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogError("SFXManager requires an AudioSource component.");
            }
        }
    }

    public void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}