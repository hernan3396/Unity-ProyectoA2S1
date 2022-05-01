using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemy Data")]
public class EnemyData : ScriptableObject
{
    public int HP;
    public int DAMAGE;
    public float FIRERATE; // cadencia de disparo
    public float ACCURACY; // que tan rapido es en apuntar
    public int MOVSPEED; // velocidad de movimiento
}