using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }
    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();

        if (instance == null) // Keep the audio source even when we go to a new scene.
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else if (instance != null && instance != this) // Removes duplicate game objects so there's only one instance of the sound manager loading.
            Destroy(gameObject);
    }
    public void PlaySound(AudioClip _sound)
    {
        source.PlayOneShot(_sound);
    }
}