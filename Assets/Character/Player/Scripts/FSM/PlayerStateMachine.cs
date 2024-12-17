using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    [field: SerializeField] public PlayerState _CurrentState { get; private set; }

    // states
    #region States
    public PlayerIdleState _IdleState { get; private set; }
    public PlayerMoveState _MoveState { get; private set; }
    public PlayerFallingState _FallingState { get; private set; }
    public PlayerJumpState _JumpState { get; private set; }
    public PlayerLandingState _LandingState { get; private set; }
    public PlayerSpellslingingSuperState _SpellslingState { get; private set; }
    public PlayerAirStepState _AirDashState { get; private set; }
    public PlayerAirJumpState _AirJumpState { get; private set; }

    #region Hurt
    public PlayerHitState _HitState { get; private set; }
	#endregion

	#endregion
	public void InitializeStateMachine(PlayerCharacter player)
    {
        _IdleState = ScriptableObject.CreateInstance<PlayerIdleState>();
        _IdleState.InitializeState(player, "idle", this);

        _MoveState = ScriptableObject.CreateInstance<PlayerMoveState>();
        _MoveState.InitializeState(player, "move", this);

        _FallingState = ScriptableObject.CreateInstance<PlayerFallingState>();
        _FallingState.InitializeState(player, "air", this);

        _JumpState = ScriptableObject.CreateInstance<PlayerJumpState>();
        _JumpState.InitializeState(player, "jump", this);

        _LandingState = ScriptableObject.CreateInstance<PlayerLandingState>();
        _LandingState.InitializeState(player, "land", this);

        _SpellslingState = ScriptableObject.CreateInstance<PlayerSpellslingingSuperState>();
        _SpellslingState.InitializeState(player, "aim", this);

        _AirDashState = ScriptableObject.CreateInstance<PlayerAirStepState>();
        _AirDashState.InitializeState(player, "airDash", this);

        _AirJumpState = ScriptableObject.CreateInstance<PlayerAirJumpState>();
        _AirJumpState.InitializeState(player, "airJump", this);

		InitializeHurtStates(player);

        _CurrentState = _IdleState;
        // Debug.Log("Starting state machine with " + state._StateAnimationName);
        _CurrentState.EnterState();
    }

	private void InitializeHurtStates(PlayerCharacter player)
	{
        _HitState = ScriptableObject.CreateInstance<PlayerHitState>();
        _HitState.InitializeState(player, "hit", this);
	}

	public void ChangeState(PlayerState state)
    {
        _CurrentState.ExitState();
        _CurrentState = state;
        _CurrentState.EnterState();
    }

	private void Update()
	{
		_CurrentState?.LogicUpdate();
	}
	private void FixedUpdate()
	{
		_CurrentState?.PhysicsUpdate();
	}
	private void LateUpdate()
	{
		_CurrentState?.LateUpdate();
	}
	// end
}
