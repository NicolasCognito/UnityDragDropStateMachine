namespace App.Utility.StateMachine
{
    public abstract class BaseState : IState
    {
        protected readonly IStateMachine stateMachine;

        protected BaseState(IStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        public virtual void Enter() { }
        public virtual void Exit() { }
        public virtual void Update() { }
        public virtual void FixedUpdate() { }
    }
}
