using UnityEngine;
using System.Collections.Generic;

public class LanternSystem : MonoBehaviour
{
    public static LanternSystem Instance { get; private set; }

    private string _lastLanternID = "";
    private Vector3 _respawnPosition = Vector3.zero;
    private List<string> _litLanterns = new List<string>();

    // Events
    public event System.Action OnPlayerRested;

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

    public void Rest(string lanternID, Vector3 position)
    {
        _lastLanternID = lanternID;
        _respawnPosition = position;

        if (!_litLanterns.Contains(lanternID))
        {
            _litLanterns.Add(lanternID);
            Debug.Log($"New lantern lit: {lanternID}");
        }

        EchoSystem.Instance?.OnPlayerRested();
        OnPlayerRested?.Invoke();

        Debug.Log($"Player rested at lantern: {lanternID}");
    }

    public Vector3 GetRespawnPosition() => _respawnPosition;
    public bool IsLanternLit(string id) => _litLanterns.Contains(id);
    public string LastLanternID => _lastLanternID;
}
