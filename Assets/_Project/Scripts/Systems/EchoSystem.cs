using UnityEngine;

public class EchoSystem : MonoBehaviour
{
    public static EchoSystem Instance { get; private set; }

    private int _currentEchoes = 0;
    private int _corpseEchoes = 0;
    private bool _corpseExists = false;
    private Vector3 _corpsePosition;

    // Events
    public event System.Action<int> OnEchoesChanged;
    public event System.Action<bool> OnCorpseChanged; // bool = corpse exists

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

    public void AddEchoes(int amount)
    {
        _currentEchoes += amount;
        OnEchoesChanged?.Invoke(_currentEchoes);
        Debug.Log($"Echoes: +{amount} | Total: {_currentEchoes}");
    }

    public void OnPlayerDied(Vector3 deathPosition)
    {
        // Ako postoji stari corpse — ti echoes su izgubljeni
        if (_corpseExists)
        {
            Debug.Log($"Previous corpse lost! {_corpseEchoes} echoes gone forever.");
        }

        _corpseEchoes = _currentEchoes;
        _corpsePosition = deathPosition;
        _currentEchoes = 0;
        _corpseExists = _corpseEchoes > 0;

        OnEchoesChanged?.Invoke(_currentEchoes);
        OnCorpseChanged?.Invoke(_corpseExists);

        if (_corpseExists)
        {
            Debug.Log($"Corpse spawned with {_corpseEchoes} echoes at {_corpsePosition}");
            SpawnCorpsePickup();
        }
    }

    public void RetrieveCorpse()
    {
        if (!_corpseExists) return;

        AddEchoes(_corpseEchoes);
        _corpseEchoes = 0;
        _corpseExists = false;

        OnCorpseChanged?.Invoke(false);
        Debug.Log("Corpse retrieved!");
    }

    public void OnPlayerRested()
    {
        // Echoes se ne gube na restu — samo enemiji respawnaju
        Debug.Log($"Player rested. Echoes kept: {_currentEchoes}");
    }

    private void SpawnCorpsePickup()
    {
        // Za sad samo log — later ćemo dodati vizualni pickup objekt
        Debug.Log($"[TODO] Spawn corpse pickup at {_corpsePosition}");
    }

    public int CurrentEchoes => _currentEchoes;
    public bool CorpseExists => _corpseExists;
    public Vector3 CorpsePosition => _corpsePosition;
}
