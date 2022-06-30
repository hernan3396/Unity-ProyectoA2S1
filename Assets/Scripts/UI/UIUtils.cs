using UnityEngine;

public class UIUtils : MonoBehaviour
{
    public void StopAnim()
    {
        gameObject.SetActive(false);
    }

    private AudioManager _audioManager;

    private void Start()
    {
        _audioManager = AudioManager.GetInstance;
    }

    public void PlaySound()
    {
        _audioManager.PlaySound(AudioManager.AudioList.Menu, false, 0);
    }
}
