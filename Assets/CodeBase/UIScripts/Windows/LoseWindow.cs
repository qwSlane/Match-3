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
        [SerializeField] private Button _restart;

        [Inject]
        public void Construct(StateMachine stateMachine)
        {
            _restart.onClick.AddListener(stateMachine.Enter<RestartLevelState>);
        }

        protected override void SelfOpen()
        {
        }

        protected override void SelfClose()
        {
        }
    }
}