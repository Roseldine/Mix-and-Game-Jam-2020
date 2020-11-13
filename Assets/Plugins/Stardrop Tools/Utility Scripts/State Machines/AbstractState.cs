
using UnityEngine;

namespace StardropTools.StateMachine
{

    //[CreateAssetMenu(fileName = "New State", menuName = "Finite State Machine / Create New State")]
    public abstract class AbstractState : MonoBehaviour
    {
        public enum ExecutionState { None, Activate, Completed, Paused }
        public ExecutionState stateExecutionFase { get; protected set; }


        public virtual void OnEnable()
        {
            stateExecutionFase = ExecutionState.None;
        }

        public abstract void HandleInput();


        public virtual bool EnterState()
        {
            if (stateExecutionFase != ExecutionState.Activate)
                stateExecutionFase = ExecutionState.Activate;

            return true;
        }

        public abstract void UpdateState();

        public virtual void PauseState()
        {
            if (stateExecutionFase != ExecutionState.Paused)
                stateExecutionFase = ExecutionState.Paused;
        }

        public virtual bool ExitState()
        {
            if (stateExecutionFase != ExecutionState.Completed)
                stateExecutionFase = ExecutionState.Completed;

            return true;
        }
    }
}
