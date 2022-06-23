using UnityEngine;

public class ExternalSound : MonoBehaviour
{
    [SerializeField] private AudioScriptable _audioScript;
    [SerializeField] private AnimationCurve _animCurve;
    private AudioManager _audioManager;

    private void Start()
    {
        _audioManager = AudioManager.GetInstance;
    }

    public void PlaySFX()
    {
        _audioManager.CreateSoundAndPlay(transform.position, _audioScript, _animCurve);
    }
}
