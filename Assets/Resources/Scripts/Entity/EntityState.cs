
using UnityEngine;
using StardropTools.StateMachine;

public class EntityState : AbstractState
{
    public EntityNew _entity;
    public EntityNewStateMachine _stateMachine;
    public int _stateId;
    public float _timeInState;
    public float _timeLimit;

    public override bool EnterState()
    {
        base.EnterState();
        _timeInState = 0;

        return true;
    }


    public override void HandleInput()
    {

    }


    public override void UpdateState()
    {
        if (_timeLimit > 0)
            _timeInState += Time.deltaTime;

        Debug.Log("Updating State");
    }


    public override bool ExitState()
    {
        base.ExitState();

        return true;
    }


    /// <summary>
    /// [Tooltip("0-idle, 1-searching, 2-moving, 3-shoot, 4-death")]
    /// </summary>
    public void ChangeState(int id) => _stateMachine.ChangeState(_stateMachine._states[id]);
}