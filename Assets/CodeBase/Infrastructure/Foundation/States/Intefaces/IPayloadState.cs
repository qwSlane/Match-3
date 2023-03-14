namespace CodeBase.Infrastructure.Foundation.States.Intefaces
{
    public interface IPayloadState<TPayload> : IExitableState
    {
        public void Enter(TPayload payload);
    }
}