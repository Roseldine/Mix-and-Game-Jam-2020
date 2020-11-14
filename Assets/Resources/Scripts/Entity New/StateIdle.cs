using System.Collections;
using UnityEngine;


public class StateIdle : EntityState
{
    public override bool EnterState()
    {
        base.EnterState();
        _entity._animation.PlayAnimation(0);

        return true;
    }

    public override void HandleInput()
    {
        if (_entity._entityType == IEntity.entityType.player)
        {
            if (InputManager.Instance._hasMovementInput)
                _stateMachine.ChangeEntityState(_stateMachine._states[2]);

            _entity.PlayerMovement();
        }
    }

    public override void UpdateState()
    {
        _entity._animation.PlayAnimation(0);

        if (_timeLimit > 0)
            _timeInState += Time.deltaTime;
    }

    public override bool ExitState()
    {
        base.ExitState();

        return true;
    }
}