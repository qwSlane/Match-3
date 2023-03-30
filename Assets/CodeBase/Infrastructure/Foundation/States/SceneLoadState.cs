// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using CodeBase.Infrastructure.Foundation.States.Intefaces;

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