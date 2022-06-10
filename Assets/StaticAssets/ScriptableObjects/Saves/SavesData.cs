using UnityEngine;

[CreateAssetMenu(fileName = "SavesData", menuName = "Saves Data", order = 0)]
public class SavesData : ScriptableObject
{
    public Vector2 CurrentCheckpoint;
    public string CurrentLevel;
    public int BulletAmount;
    public int RocketAmount;
    public int Health;
}