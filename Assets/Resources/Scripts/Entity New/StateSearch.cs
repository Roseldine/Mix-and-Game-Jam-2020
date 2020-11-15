using System.Collections;
using UnityEngine;

public class StateSearch : EntityState
{
    public override bool EnterState()
    {
        base.EnterState();
        UpdateEnemy();

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


    void UpdateEnemy()
    {
        if (_entity._entityType == IEntity.entityType.enemy)
        {
            if (_entity._hasPath == false)
                _entity.SetAgentPath(Trophy.Instance._trophyTransform.position);

            if (_entity._hasPath == true)
                ChangeState(2);
        }
    }
}