namespace App.Utility.StateMachine
{
    public class StateMachine : IStateMachine
    {
        private IState currentState;

        public IState CurrentState => currentState;

        public void Initialize(IState startingState)
        {
            currentState = startingState;
            currentState.Enter();
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
            if (currentState != null)
            {
                currentState.Update();
            }
        }

        public void FixedUpdate()
        {
            if (currentState != null)
            {
                currentState.FixedUpdate();
            }
        }
    }
}
