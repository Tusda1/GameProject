using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource soundEffectAudioSource;
    
    [Header("Sound Effects")]
    [SerializeField] private AudioClip writingSound;

    private void Start()
    {
        // Subscribe to events
        GameEvents.playWritingSound += PlayWritingSound;
    }

    private void OnDestroy()
    {
        // Unsubscribe from events
        GameEvents.playWritingSound -= PlayWritingSound;
    }

    private void PlayWritingSound()
    {
        soundEffectAudioSource.clip = writingSound;
        soundEffectAudioSource.Play();
    }
}
