using CodeBase.Infrastructure.Foundation.States.Intefaces;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.Foundation.States
{
    public class SceneLoadState : IPayloadState<string>
    {
        private readonly StateMachine _stateMachine;

        public SceneLoadState(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public async void Enter(string payload)
        {
            await LoadLevel(payload);
        }

        public async UniTask LoadLevel(string sceneName)
        {
            await SceneManager.LoadSceneAsync(sceneName);
        }

        public void Exit()
        {
        }
    }
}