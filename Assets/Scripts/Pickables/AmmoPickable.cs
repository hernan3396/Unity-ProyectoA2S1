using UnityEngine;

public class AmmoPickable : MonoBehaviour
{
    private Rigidbody _rb;
    private InventoryManager _invManage;
    [SerializeField] private int _bulletRestockAmount;
    [SerializeField] private int _rocketRestockAmount;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _invManage = GameManager.GetInstance.GetInvManager;
    }

    private void DesactivateAmmo()
    {
        _rb.velocity = Vector3.zero;
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Debug.Log("agarrado");
            _invManage.AddAmount((int)InventoryManager.ItemID.Bullet, _bulletRestockAmount);
            _invManage.AddAmount((int)InventoryManager.ItemID.Rocket, _rocketRestockAmount);
            DesactivateAmmo();
            return;
        }

        if (other.gameObject.CompareTag("Floor"))
        {
            _rb.velocity = Vector3.zero;
            return;
        }
    }
}
