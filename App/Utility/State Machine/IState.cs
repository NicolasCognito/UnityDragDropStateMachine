namespace App.Utility.StateMachine
{
    public interface IState
    {
        void Enter();
        void Exit();
        void Update();
        void FixedUpdate();
    }
}
