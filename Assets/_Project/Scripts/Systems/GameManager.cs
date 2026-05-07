using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private PlayerHealth _playerHealth;
    private EchoSystem _echoSystem;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _playerHealth = FindObjectOfType<PlayerHealth>();
        _echoSystem = EchoSystem.Instance;

        if (_playerHealth != null)
        {
            _playerHealth.OnDied += HandlePlayerDeath;
        }
    }

    private void HandlePlayerDeath()
    {
        Vector3 deathPosition = _playerHealth.transform.position;
        _echoSystem?.OnPlayerDied(deathPosition);

        Debug.Log("Game Manager: Player died — starting respawn...");
        StartCoroutine(RespawnRoutine());
    }

    private System.Collections.IEnumerator RespawnRoutine()
    {
        // Kratka pauza prije respawna
        yield return new WaitForSeconds(2f);

        // Respawnaj na zadnjem lanternu
        Vector3 respawnPos = LanternSystem.Instance != null
            ? LanternSystem.Instance.GetRespawnPosition()
            : Vector3.zero;

        _playerHealth.transform.position = respawnPos;

        // Restoriraj HP
        _playerHealth.Heal(_playerHealth.MaxHP);

        Debug.Log($"Player respawned at {respawnPos}");
    }
}
