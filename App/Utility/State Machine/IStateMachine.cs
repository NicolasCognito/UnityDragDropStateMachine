namespace App.Utility.StateMachine
{
    public interface IStateMachine
    {
        IState CurrentState { get; }
        void Initialize(IState startingState);
        void TransitionTo(IState nextState);
        void Update();
        void FixedUpdate();
    }
}
