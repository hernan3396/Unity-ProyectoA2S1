using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ExternalSound : MonoBehaviour
{
    [SerializeField] private AudioScriptable _audioScript;
    private AudioManager _audioManager;
    private AudioSource _audioSource;

    private void Start()
    {
        _audioManager = AudioManager.GetInstance;
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlaySFX()
    {
        _audioManager.RandomizeExternalSound(_audioSource, _audioScript);
    }
}
