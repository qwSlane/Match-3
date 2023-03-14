using CodeBase.Infrastructure.Foundation.States.Intefaces;

namespace CodeBase.Infrastructure.Foundation.States
{
    public class InitLevelState: IPayloadState<string>
    {

        public void Enter(string payload)
        {
            
        }

        public void Exit()
        {
            throw new System.NotImplementedException();
        }
    }
}