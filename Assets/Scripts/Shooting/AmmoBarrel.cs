using UnityEngine;

public class AmmoBarrel : MonoBehaviour
{
    [SerializeField] private Transform _spawnPosition;
    [SerializeField] private Vector2 _randomDirection;
    [SerializeField] private int _force;
    private PoolManager _ammoPool;

    private void Start()
    {
        _ammoPool = GameManager.GetInstance.GetAmmoPool;
    }

    public void SpawnAmmo()
    {
        GameObject ammo = _ammoPool.GetPooledObject();
        if (!ammo) return;
        
        ammo.transform.position = (Vector2)_spawnPosition.position;
        ammo.SetActive(true);

        Vector2 direction = new Vector2(Random.Range(_randomDirection.x, _randomDirection.y), _force);
        ammo.GetComponent<Rigidbody>().AddForce(direction, ForceMode.Impulse);
    }
}
