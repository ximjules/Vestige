using UnityEngine;

public class RallySystem : MonoBehaviour
{
    [Header("Rally Settings")]
    public float RallyWindowSeconds = 3f;
    public float RallyRecoveryRatio = 0.4f; // 40% of damage received

    private float _rallyPool = 0f;
    private float _rallyTimer = 0f;
    private bool _rallyActive = false;

    private PlayerHealth _health;

    // Events
    public event System.Action<float> OnRallyPoolChanged; // float = rally amount

    private void Awake()
    {
        _health = GetComponent<PlayerHealth>();
    }

    // Called by PlayerHealth when damage is received
    public void OnDamageReceived(float damageAmount)
    {
        _rallyPool = damageAmount * RallyRecoveryRatio;
        _rallyTimer = RallyWindowSeconds;
        _rallyActive = true;

        OnRallyPoolChanged?.Invoke(_rallyPool);
        Debug.Log($"Rally active! Can recover: {_rallyPool} HP");
    }

    // Called by LightAttackState when player lands a hit
    public void TryConsumeRally()
    {
        if (!_rallyActive) return;

        float recovered = _rallyPool;
        _rallyPool = 0f;
        _rallyActive = false;

        _health.Heal(recovered);
        OnRallyPoolChanged?.Invoke(0f);
        Debug.Log($"Rally! Recovered: {recovered} HP");
    }

    private void Update()
    {
        if (!_rallyActive) return;

        _rallyTimer -= Time.deltaTime;
        if (_rallyTimer <= 0f)
        {
            _rallyPool = 0f;
            _rallyActive = false;
            OnRallyPoolChanged?.Invoke(0f);
            Debug.Log("Rally window expired!");
        }
    }
}