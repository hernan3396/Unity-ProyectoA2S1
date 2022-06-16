using UnityEngine;

public class AmmoPickable : MonoBehaviour
{
    private Rigidbody _rb;
    private InventoryManager _invManage;
    private AudioManager _audioManager;
    [SerializeField] private int _bulletRestockAmount;
    [SerializeField] private int _rocketRestockAmount;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _invManage = GameManager.GetInstance.GetInvManager;
        _audioManager = AudioManager.GetInstance;
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
            _invManage.AddAmount((int)InventoryManager.ItemID.PlayerBullet, _bulletRestockAmount);
            _invManage.AddAmount((int)InventoryManager.ItemID.Rocket, _rocketRestockAmount);
            _audioManager.PlaySound(AudioManager.AudioList.Pickables, false, 1);
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
