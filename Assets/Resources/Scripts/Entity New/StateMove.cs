
using System.Collections;
using UnityEngine;

public class StateMove : EntityState
{
    public override bool EnterState()
    {
        base.EnterState();
        _entity._animation.PlayAnimation(1);

        return true;
    }

    public override void HandleInput()
    {
        if (_entity._entityType == IEntity.entityType.player)
        {
            if (InputManager.Instance._hasMovementInput == false)
                _stateMachine.ChangeEntityState(_stateMachine._states[0]);

            _entity.PlayerMovement();
            _entity.CameraFollowPlayer(1);
        }
    }

    public override void UpdateState()
    {
        if (_timeLimit > 0)
            _timeInState += Time.deltaTime;

        if (_entity._isGrounded)
            _entity._animation.PlayAnimation(1);
        else
            _entity._animation.PlayAnimation(2);
    }

    public override bool ExitState()
    {
        base.ExitState();

        return true;
    }
}