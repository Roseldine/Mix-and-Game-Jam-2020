
using UnityEngine;

namespace StardropTools.StateMachine
{
    public class FiniteStateMachine : MonoBehaviour
    {
        [Header("State Viewer")]
        public AbstractState _startingState;
        public AbstractState _currentState;
        public AbstractState _previousState;

        [Header("Debug")]
        public bool log;



        #region Constructors
        public FiniteStateMachine()
        {
            Debug.LogWarning("Created a stateless FSM");
        }

        public FiniteStateMachine(AbstractState initialState)
        {
            _startingState = initialState;
        }
        #endregion



        #region Initialization
        public virtual void Start()
        {
            if (_startingState != null)
                ChangeState(_startingState);
        }


        public virtual void InitializeFSM(AbstractState initialState)
        {
            _startingState = initialState;
            ChangeState(_startingState);
        }
        #endregion


        public virtual void Update()
        {
            if (_currentState != null)
            {
                _currentState.UpdateState();
            }

            else
            {
                Debug.LogWarning(name + " FSM Doesn't have a State");
                return;
            }
        }




        #region State Changes

        public virtual void ChangeState(AbstractState nextState)
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


        public virtual void ChangeToPreviewsState()
        {
            if (_previousState != null)
            {
                _currentState.ExitState();

                var _tempState = _currentState;
                _currentState = _previousState;
                _previousState = _tempState;
            }

            else
            {
                Debug.Log("There is no previews state");
                return;
            }
        }
        #endregion
    }
}
