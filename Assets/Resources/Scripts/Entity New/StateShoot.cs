using System.Collections;
using UnityEngine;

public class StateShoot : EntityState
{
    public override bool EnterState()
    {
        base.EnterState();
        PlaySportAnimation();
        _entity._canShoot = false;

        return true;
    }

    public override void HandleInput()
    {

    }

    public override void UpdateState()
    {
        if (_timeLimit > 0)
            _timeInState += Time.deltaTime;

        var _animInfo = _entity._animator.GetCurrentAnimatorStateInfo(0);
        var _animTransInfo = _entity._animator.GetAnimatorTransitionInfo(0);

        if (_animTransInfo.anyState == false && _animInfo.IsTag(_entity._animation._animTags[1]) == true)
        {
            if (_entity._animator.GetCurrentAnimatorStateInfo(0).normalizedTime > .95f)
                _stateMachine.ChangeToPreviewsState();
        }
    }

    public override bool ExitState()
    {
        base.ExitState();
        _entity._canShoot = true;

        return true;
    }


    public void PlaySportAnimation()
    {
        int _animId = 0;

        if (_entity._entitySport == IEntity.entitySport.basketball)
            _animId = 4;

        if (_entity._entitySport == IEntity.entitySport.football)
            _animId = 5;

        if (_entity._entitySport == IEntity.entitySport.baseball)
            _animId = 6;

        _entity._animation.PlayAnimation(_animId - 1);
    }
}