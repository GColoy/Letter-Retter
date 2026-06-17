using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Slots")]
    public AudioClip main;
    public AudioClip combat;

    private AudioSource audioSource;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
    }

    public void Play(string slot)
    {
        AudioClip clip = slot == "main" ? main : combat;

        if (clip == null)
        {
            Debug.LogWarning($"Slot {slot} hat keinen AudioClip!");
            return;
        }

        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void Stop() => audioSource.Stop();
}