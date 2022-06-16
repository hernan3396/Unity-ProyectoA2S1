using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "AudioItem", menuName = "Audio/Create audio item", order = 0)]
public class AudioScriptable : ScriptableObject
{
    public List<AudioClip> clips;
    public bool IsMusic = false;

    public Vector2 volume = new Vector2(1, 1);
    public Vector2 pitch = new Vector2(1, 1);

    public AudioClip GetAudioClip(int index)
    {
        return clips[index];
    }

    public AudioClip GetRandom()
    {
        return clips[Random.Range(0, clips.Count)];
    }
}