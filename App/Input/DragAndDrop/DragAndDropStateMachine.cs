using System;
using System.Collections.Generic;
using App.Utility.StateMachine;

namespace InputSystem
{

    public class DragDropStateMachine : IStateMachine
    {
        private IState currentState;
        private readonly Dictionary<Type, Dictionary<DragDropTrigger, Type>> transitions;
        private readonly Dictionary<Type, IState> stateCache;

        public IState CurrentState => currentState;

        public DragDropStateMachine()
        {
            transitions = new Dictionary<Type, Dictionary<DragDropTrigger, Type>>();
            stateCache = new Dictionary<Type, IState>();
        }

        public void RegisterTransition<TFromState, TToState>(DragDropTrigger trigger)
            where TFromState : IState
            where TToState : IState
        {
            var fromStateType = typeof(TFromState);
            
            if (!transitions.ContainsKey(fromStateType))
            {
                transitions[fromStateType] = new Dictionary<DragDropTrigger, Type>();
            }

            transitions[fromStateType][trigger] = typeof(TToState);
        }

        public void RegisterState<TState>(TState state) where TState : IState
        {
            var stateType = typeof(TState);
            if (!stateCache.ContainsKey(stateType))
            {
                stateCache[stateType] = state;
            }
        }

        public void Initialize(IState startingState)
        {
            RegisterState(startingState);
            currentState = startingState;
            currentState.Enter();
        }

        public void HandleTrigger(DragDropTrigger trigger)
        {
            var currentStateType = currentState.GetType();
            
            if (transitions.ContainsKey(currentStateType) &&
                transitions[currentStateType].ContainsKey(trigger))
            {
                var nextStateType = transitions[currentStateType][trigger];
                if (stateCache.ContainsKey(nextStateType))
                {
                    TransitionTo(stateCache[nextStateType]);
                }
            }
        }

        public void TransitionTo(IState nextState)
        {
            if (currentState != null)
            {
                currentState.Exit();
            }

            currentState = nextState;
            currentState.Enter();
        }

        public void Update()
        {
            currentState?.Update();
        }

        public void FixedUpdate()
        {
            currentState?.FixedUpdate();
        }
    }
}