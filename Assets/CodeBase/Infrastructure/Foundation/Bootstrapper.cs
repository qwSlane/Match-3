// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using UnityEngine;
using Zenject;
using CodeBase.Infrastructure.Foundation.States;

namespace CodeBase.Infrastructure.Foundation
{
    public class Bootstrapper : MonoBehaviour
    {
        private const string SceneName = "Game";
    
        private StateMachine _stateMachine;

        [Inject]
        public void Initialize(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _stateMachine.Enter<SceneLoadState, string>(SceneName);
        }
    }
}