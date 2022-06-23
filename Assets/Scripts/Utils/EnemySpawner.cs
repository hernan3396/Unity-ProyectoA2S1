using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // este script lo hice medio cutre porque solo
    // lo planee para el boss, aunque si se usa en
    // otro lado y da problemas no me hago cargo 😎
    // mismo la implementacion del boss con esto
    // es media cutre uwu
    [SerializeField] private Transform _enemy; // enemy to spawn
    [SerializeField] private Transform[] _spawnPoint;
    private GameObject[] _enemiesSpawned; // para luego destruirlos si perdes o se reinicia el juego

    public void SpawnEnemy(int index)
    {
        Instantiate(_enemy, _spawnPoint[index].position, Quaternion.identity);
    }

    public void DestroyEnemies()
    {
        foreach (GameObject enemy in _enemiesSpawned)
            Destroy(enemy.gameObject);
    }
}
