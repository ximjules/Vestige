using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Sub-systems
    public Rigidbody2D Rb { get; private set; }
    public Animator Animator { get; private set; }
    public PlayerStateMachine StateMachine { get; private set; }

    // Stats
    [Header("Movement")]
    public float MoveSpeed = 6f;
    public float JumpForce = 14f;

    [Header("Dash")]
    public float DashForce = 20f;
    public float DashDuration = 0.2f;
    public float DashCooldown = 1.2f;

    public bool DashPressed { get; private set; }
    public bool CanDash { get; private set; } = true;

    [Header("Combat")]
    public float LightAttackDamage = 15f;
    public float LightAttackRange = 1.8f;
    public float LightAttackCooldown = 0.4f;
    public LayerMask EnemyLayer;

    public bool AttackPressed { get; private set; }
    public bool CanAttack { get; private set; } = true;

    [Header("Ground Check")]
    public Transform GroundCheck;
    public float GroundCheckRadius = 0.2f;
    public LayerMask GroundLayer;

    // Input (čitamo u Update, koriste stanja)
    public Vector2 MoveInput { get; private set; }
    public bool JumpPressed { get; private set; }

    // State
    public bool IsGrounded { get; private set; }
    public bool IsFacingRight { get; private set; } = true;
    public StaminaSystem Stamina { get; private set; }

    // Events
    public event System.Action<float> OnPlayerDamaged;
    public event System.Action OnPlayerDied;

    private void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        StateMachine = new PlayerStateMachine(this);
        StateMachine.Initialize(new IdleState());
        Stamina = GetComponent<StaminaSystem>();
    }

    private void Update()
    {
        // Čitanje inputa
        MoveInput = new Vector2(
            UnityEngine.InputSystem.Keyboard.current.dKey.isPressed ? 1f :
            UnityEngine.InputSystem.Keyboard.current.aKey.isPressed ? -1f : 0f, 0f);

        JumpPressed = UnityEngine.InputSystem.Keyboard.current.spaceKey.wasPressedThisFrame;
        DashPressed = UnityEngine.InputSystem.Keyboard.current.leftShiftKey.wasPressedThisFrame;
        AttackPressed = UnityEngine.InputSystem.Keyboard.current.jKey.wasPressedThisFrame;

        // Ground check
        IsGrounded = Physics2D.OverlapCircle(
            GroundCheck.position, GroundCheckRadius, GroundLayer);

        StateMachine.Update();
        HandleFlip();
    }

    private void FixedUpdate()
    {
        StateMachine.FixedUpdate();
    }

    private void HandleFlip()
    {
        if (MoveInput.x > 0 && !IsFacingRight) Flip();
        else if (MoveInput.x < 0 && IsFacingRight) Flip();
    }

    private void Flip()
    {
        IsFacingRight = !IsFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void TakeDamage(float amount)
    {
        OnPlayerDamaged?.Invoke(amount);
    }

    public void StartDashCooldown()
    {
        StartCoroutine(DashCooldownRoutine());
    }

    private System.Collections.IEnumerator DashCooldownRoutine()
    {
        CanDash = false;
        yield return new WaitForSeconds(DashCooldown);
        CanDash = true;
    }

    public void StartAttackCooldown()
    {
        StartCoroutine(AttackCooldownRoutine());
    }

    private System.Collections.IEnumerator AttackCooldownRoutine()
    {
        CanAttack = false;
        yield return new WaitForSeconds(LightAttackCooldown);
        CanAttack = true;
    }
}
