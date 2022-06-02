using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public enum AudioList
    {
        AllMusic
    }

    [SerializeField] private List<AudioCompressionFormat> _audioList;

    #region Sources
    [Header("Sources")]
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _soundSource;
    #endregion

    #region Mixer
    [Header("Mixer")]
    [SerializeField] private AudioMixer _mixer;
    private bool _isFading = false;
    #endregion

    private static AudioManager _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void PlaySfx()
    {

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
