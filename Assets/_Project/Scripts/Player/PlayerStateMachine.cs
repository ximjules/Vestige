public class PlayerStateMachine
{
    public IPlayerState CurrentState { get; private set; }

    private PlayerController _player;

    public PlayerStateMachine(PlayerController player)
    {
        _player = player;
    }

    public void Initialize(IPlayerState startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter(_player);
    }

    public void ChangeState(IPlayerState newState)
    {
        CurrentState.Exit(_player);
        CurrentState = newState;
        CurrentState.Enter(_player);
    }

    public void Update()
    {
        CurrentState?.Update(_player);
    }

    public void FixedUpdate()
    {
        CurrentState?.FixedUpdate(_player);
    }
}