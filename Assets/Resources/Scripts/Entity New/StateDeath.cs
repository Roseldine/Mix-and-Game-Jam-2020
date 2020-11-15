using System.Collections;
using UnityEngine;

public class StateDeath : EntityState
{
    public override bool EnterState()
    {
        base.EnterState();
        _entity.PlayAudio(1);
        _entity._animation.PlayAnimation(6);
        return true;
    }

    public override void HandleInput()
    {

    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (_timeInState > _timeLimit)
        {
            // start particles
            Destroy(_entity.gameObject);
        }
    }

    public override bool ExitState()
    {
        base.ExitState();

        return true;
    }
}