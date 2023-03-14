namespace CodeBase.Infrastructure.Foundation.States.Intefaces
{
    public interface IState : IExitableState
    {
        public void Enter();
    }
}