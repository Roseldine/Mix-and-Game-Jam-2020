
using UnityEngine;
using StardropTools.StateMachine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class EntityNewStateMachine : FiniteStateMachine
{
    public EntityNew _entity;
    public Transform _stateContainer;
    [Tooltip("0-idle, 1-searching, 2-moving, 3-shoot, 4-death")]
    public EntityState[] _states;

    public void UpdateStateMachine()
    {
        if (_currentState != null)
        {
            _currentState.UpdateState();
            _currentState.HandleInput();
        }
    }

    public void ChangeEntityState(EntityState nextState)
    {
        if (_currentState != null)
        {
            _currentState.ExitState();
            _previousState = _currentState;
        }

        _currentState = nextState;
        _currentState.EnterState();

        if (log == true)
            Debug.Log("<color=yellow> Changed to state: </color> <color=cyan>" + _currentState + "</color>");

    }

    public void GetStates()
    {
        if (_entity == null)
            _entity = GetComponentInParent<EntityNew>();

        if (_stateContainer == null)
            _stateContainer = transform;

        _states = GetComponentsInChildren<EntityState>();

        for (int i = 0; i < _states.Length; i++)
        {
            var state = _states[i];
            state._stateMachine = this;
            state._entity = _entity;
            state._stateId = i;
        }

        _startingState = _states[0];
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(EntityNewStateMachine))]
public class CE_EntityStateMachine : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EntityNewStateMachine _target = (EntityNewStateMachine)target;

        if (GUILayout.Button("Get States"))
            _target.GetStates();
    }
}

#endif