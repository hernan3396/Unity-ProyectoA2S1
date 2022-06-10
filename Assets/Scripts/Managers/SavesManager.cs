using UnityEngine;
using UnityEngine.SceneManagement;

public class SavesManager : MonoBehaviour
{
    [SerializeField] private SavesData _savesData;
    private InventoryManager _invManager; // referencia a la # de balas
    private Player _player; // referencia a los stats del player
    // se usa para cuando guardas un checkpoint
    // ver que valores tenia

    private void Awake()
    {
        // _currentCheckpoint = new Vector2(PlayerPrefs.GetFloat("SCC_CheckpointX"), PlayerPrefs.GetFloat("SCC_CheckpointY"));
        // Debug.Log(_currentCheckpoint);
        LoadData();
    }

    private void Start()
    {
        // ya que esta este me parecio al pedo crear 
        // otra referencia en el game manager
        _player = GameManager.GetInstance.GetPlayerPos.GetComponent<Player>();
        _invManager = GameManager.GetInstance.GetInvManager;
    }

    private void LoadData()
    {
        _savesData.CurrentCheckpoint = new Vector2(PlayerPrefs.GetFloat("SCC_CheckpointX"), PlayerPrefs.GetFloat("SCC_CheckpointY"));
        _savesData.Health = PlayerPrefs.GetInt("SCC_Health");
        _savesData.CurrentLevel = SceneManager.GetActiveScene().name;
    }

    public void SetCheckpoint(Transform destination)
    {
        PlayerPrefs.SetFloat("SCC_CheckpointX", destination.position.x);
        PlayerPrefs.SetFloat("SCC_CheckpointY", destination.position.y);
        _savesData.CurrentCheckpoint = destination.position;

        _savesData.Health = _player.GetHealth;
        PlayerPrefs.SetInt("SCC_Health", _savesData.Health);

        _savesData.CurrentLevel = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString("SCC_Level", _savesData.CurrentLevel);

        _savesData.BulletAmount = _invManager.GetAmount((int)InventoryManager.ItemID.PlayerBullet);
        PlayerPrefs.SetInt("SCC_Bullets", _savesData.BulletAmount);

        _savesData.RocketAmount = _invManager.GetAmount((int)InventoryManager.ItemID.Rocket);
        PlayerPrefs.SetInt("SCC_Bullets", _savesData.RocketAmount);
    }

    public Vector2 GetCurrentCheckpoint
    {
        get { return _savesData.CurrentCheckpoint; }
    }

    public string GetCurrentLevel
    {
        get { return _savesData.CurrentLevel; }
    }

    public int GetBulletAmount
    {
        get { return _savesData.BulletAmount; }
    }

    public int GetRocketAmount
    {
        get { return _savesData.RocketAmount; }
    }

    public int GetHealth
    {
        get { return _savesData.Health; }
    }
}