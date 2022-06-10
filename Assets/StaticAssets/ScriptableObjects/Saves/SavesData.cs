using UnityEngine;

[CreateAssetMenu(fileName = "SavesData", menuName = "Saves Data", order = 0)]
public class SavesData : ScriptableObject
{
    public string CurrentLevel;

    [Header("Position")]
    public Vector2 InitialCheckpoint;
    public Vector2 CurrentCheckpoint;

    [Header("Ammo")]
    public int InitialBulletAmount;
    public int BulletAmount;
    public int InitialRocketAmount;
    public int RocketAmount;

    [Header("Health")]
    public int InitialHealth;
    public int Health;
}