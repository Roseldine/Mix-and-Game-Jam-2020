
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
}