using UnityEngine;
using UnityEngine.SceneManagement;

public class SavesManager : MonoBehaviour
{
    // quedo un poquito feo pero como nunca lo habia hecho
    // con un scriptable object hice un poco de freestyle
    // y al menos funciona
    [SerializeField] private SavesData _savesData;
    [SerializeField] private bool _inMenu = false;
    private InventoryManager _invManager; // referencia a la # de balas
    private Player _player; // referencia a los stats del player
    // se usa para cuando guardas un checkpoint
    // ver que valores tenia

    private void Awake()
    {
        LoadData();
    }

    private void Start()
    {
        // ya que esta este me parecio al pedo crear 
        // otra referencia en el game manager
        if (_inMenu) return;

        _player = GameManager.GetInstance.GetPlayerPos.GetComponent<Player>();
        _invManager = GameManager.GetInstance.GetInvManager;
    }

    private void LoadData()
    {
        if (PlayerPrefs.HasKey("SCC_CheckpointX") || PlayerPrefs.HasKey("SCC_CheckpointY"))
            _savesData.CurrentCheckpoint = new Vector2(PlayerPrefs.GetFloat("SCC_CheckpointX"), PlayerPrefs.GetFloat("SCC_CheckpointY"));
        else
            _savesData.CurrentCheckpoint = _savesData.InitialCheckpoint;

        // asumimos que si se guardo la vida (por ejemplo)
        // se guardaron los otros stats
        // y usamos el initial amount por si queremos arrancar el nivel con una cantidad X de
        // municion, vida, etc (ejemplo empieza sin balas)
        if (PlayerPrefs.HasKey("SCC_Health"))
        {
            _savesData.Health = PlayerPrefs.GetInt("SCC_Health");
            _savesData.BulletAmount = PlayerPrefs.GetInt("SCC_Bullets");
            _savesData.RocketAmount = PlayerPrefs.GetInt("SCC_Rockets");
        }
        else
        {
            _savesData.Health = _savesData.InitialHealth;
            _savesData.BulletAmount = _savesData.InitialBulletAmount;
            _savesData.RocketAmount = _savesData.InitialRocketAmount;
        }

        if (PlayerPrefs.HasKey("SCC_Level"))
            _savesData.CurrentLevel = PlayerPrefs.GetString("SCC_Level");
        else
            _savesData.CurrentLevel = SceneManager.GetActiveScene().name;

        // si no hay nada de eso carga los datos que tenga el _savesData
        // los cuales se configuran a mano
        // sirve para iniciar el nivel en las condiciones que querramos
    }

    public void SetCheckpoint(Transform destination)
    {
        PlayerPrefs.SetFloat("SCC_CheckpointX", destination.position.x);
        PlayerPrefs.SetFloat("SCC_CheckpointY", destination.position.y);
        _savesData.CurrentCheckpoint = destination.position;

        _savesData.CurrentLevel = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString("SCC_Level", _savesData.CurrentLevel);

        SaveStats();
    }

    public void SaveStats()
    {
        _savesData.Health = _player.GetHealth;
        PlayerPrefs.SetInt("SCC_Health", _savesData.Health);

        _savesData.BulletAmount = _invManager.GetAmount((int)InventoryManager.ItemID.PlayerBullet);
        PlayerPrefs.SetInt("SCC_Bullets", _savesData.BulletAmount);

        _savesData.RocketAmount = _invManager.GetAmount((int)InventoryManager.ItemID.Rocket);
        PlayerPrefs.SetInt("SCC_Rockets", _savesData.RocketAmount);
    }

    public void DeleteCheckpoints(bool keepStats)
    {
        PlayerPrefs.DeleteKey("SCC_CheckpointX");
        PlayerPrefs.DeleteKey("SCC_CheckpointY");
        PlayerPrefs.DeleteKey("SCC_Level");

        // si (no) queres mantener las balas en el cambio de nivel
        // para hacer que arranque sin balas x ejemplo
        if (keepStats) return;
        PlayerPrefs.DeleteKey("SCC_Health");
        PlayerPrefs.DeleteKey("SCC_Bullets");
        PlayerPrefs.DeleteKey("SCC_Rockets");
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