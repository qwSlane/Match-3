// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using UnityEngine;
using UnityEngine.UI;
using Zenject;
using CodeBase.Infrastructure.Foundation;
using CodeBase.Infrastructure.Foundation.States;

namespace CodeBase.UIScripts.Windows
{
    public class LoseWindow : Window
    {
        private const string MenuScene = "Menu";
        [SerializeField] private Button _restart;
        [SerializeField] private Button _exit;

        [Inject]
        public void Construct(StateMachine stateMachine)
        {
            _restart.onClick.AddListener(stateMachine.Enter<RestartLevelState>);
            _exit.onClick.AddListener(
                () => stateMachine.Enter<SceneLoadState, string>(MenuScene));
        }

        protected override void SelfOpen()
        {
        }

        protected override void SelfClose()
        {
        }
    }
}