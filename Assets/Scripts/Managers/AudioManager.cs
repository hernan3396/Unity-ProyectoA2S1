using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;

    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioClip _audioClip;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        PlayMusic();
    }

    public void PlayMusic()
    {
        _musicSource.clip = _audioClip;
        _musicSource.Play();
    }

    private void OnDestroy()
    {
        if (_instance != this)
        {
            _instance = null;
        }
    }

    public static AudioManager GetInstance
    {
        get { return _instance; }
    }
}
