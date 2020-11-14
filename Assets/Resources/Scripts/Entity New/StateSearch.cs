using System.Collections;
using UnityEngine;

public class StateSearch : EntityState
{
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
    }

    public override bool ExitState()
    {
        base.ExitState();

        return true;
    }
}