using System.Collections;
using UnityEngine;


public class StateIdle : EntityState
{
    public override bool EnterState()
    {
        base.EnterState();
        _entity._animation.PlayAnimation(0);
        if (_entity._entityType == IEntity.entityType.enemy)
            _entity.StopAgent();

            return true;
    }

    public override void HandleInput()
    {
        if (_entity._entityType == IEntity.entityType.player)
        {
            if (InputManager.Instance._hasMovementInput)
                ChangeState(2);

            _entity.PlayerMovement();
            _entity.CameraFollowPlayer(0);
        }
    }

    public override void UpdateState()
    {
        if (_entity._isGrounded)
            _entity._animation.PlayAnimation(0);
        else
            _entity._animation.PlayAnimation(2);

        if (_timeLimit > 0)
            _timeInState += Time.deltaTime;

        UpdateEnemy();
    }

    public override bool ExitState()
    {
        base.ExitState();

        return true;
    }


    void UpdateEnemy()
    {
        if (_entity._entityType == IEntity.entityType.enemy)
        {
            if (_entity._isAttacking == false)
            {
                if (_timeInState > _timeLimit)
                    ChangeState(1);
            }

            else
            {
                if (_timeInState > _entity._agentShootCooldown)
                    ChangeState(3);
            }
        }
    }
}