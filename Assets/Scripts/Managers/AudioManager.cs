using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public enum AudioList
    {
        // aca poner los sonidos
        OST,
        Gunshoots,
        Pickables,
        PlayerSFX
    }

    [SerializeField] private List<AudioScriptable> _audioList;

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

        PlayMusic(AudioList.OST);
    }

    public void TestSFX()
    {
        PlaySound(AudioList.Gunshoots);
    }

    public void PlaySound(AudioList audioItem, bool randomSound = false, int index = 0)
    {
        AudioScriptable audioScript = _audioList[(int)audioItem];

        if (!audioScript) return;

        _soundSource.volume = Random.Range(audioScript.volume.x, audioScript.volume.y);
        _soundSource.pitch = Random.Range(audioScript.pitch.x, audioScript.pitch.y);

        if (randomSound)
            _soundSource.PlayOneShot(audioScript.GetRandom());
        else
            _soundSource.PlayOneShot(audioScript.GetAudioClip(index));
    }

    public void PlayMusic(AudioList audioItem, bool randomSound = false, int index = 0)
    {
        if (_isFading) return;

        AudioScriptable audioScript = _audioList[(int)audioItem];
        if (!audioScript || !audioScript.IsMusic) return;

        if (randomSound)
            _musicSource.clip = audioScript.GetRandom();
        else
            _musicSource.clip = audioScript.GetAudioClip(index);

        _musicSource.volume = Random.Range(audioScript.volume.x, audioScript.volume.y);
        _musicSource.pitch = Random.Range(audioScript.pitch.x, audioScript.pitch.y);

        _musicSource.Play();
    }

    private void FadeBetweenMusic(AudioList musicClip)
    {
        _isFading = true;
        StartCoroutine("FadeOut", musicClip);
    }

    private IEnumerator FadeOut(AudioList musicClip)
    {
        float musicVolume;
        _mixer.GetFloat("MusicVolume", out musicVolume);

        while (musicVolume > -80)
        {
            _mixer.SetFloat("MusicVolume", musicVolume -= 2f);
            yield return new WaitForSeconds(0.1f);
        }

        PlayMusic(musicClip);
        StartCoroutine("FadeIn");
    }

    private IEnumerator FadeIn()
    {
        float musicVolume;
        _mixer.GetFloat("MusicVolume", out musicVolume);

        while (musicVolume < -20)
        {
            _mixer.SetFloat("MusicVolume", musicVolume += 2f);
            yield return new WaitForSeconds(0.1f);
        }

        _isFading = false;
    }

    // si bien se podrian juntar con
    // PlaySound() creo que como trata de
    // un sonido 3d es valido separarlos 
    public void RandomizeExternalSound(AudioSource audioSource, AudioScriptable audioScript)
    {
        if (!audioScript) return;
        Debug.Log("Explosion");
        Debug.Log(audioScript);

        // audioSource.clip = audioScript.GetAudioClip(0);
        audioSource.volume = Random.Range(audioScript.volume.x, audioScript.volume.y);
        audioSource.pitch = Random.Range(audioScript.pitch.x, audioScript.pitch.y);

        audioSource.PlayOneShot(audioScript.GetAudioClip(0));
    }

    public void CreateSoundAndPlay(Vector3 pos, AudioScriptable audioScript, AnimationCurve curve)
    {
        GameObject go = new GameObject();
        go.transform.position = pos;

        AudioSource source = go.AddComponent<AudioSource>();
        source.spatialBlend = 1.0f;
        source.rolloffMode = AudioRolloffMode.Custom;
        source.maxDistance = 200;
        source.SetCustomCurve(AudioSourceCurveType.CustomRolloff, curve);

        source.clip = audioScript.GetAudioClip(0);
        source.PlayOneShot(source.clip);

        Destroy(go, source.clip.length / source.pitch);
    }

    private void OnDestroy()
    {
        if (_instance != null)
        {
            _instance = null;
        }
    }

    public static AudioManager GetInstance
    {
        get { return _instance; }
    }
}
