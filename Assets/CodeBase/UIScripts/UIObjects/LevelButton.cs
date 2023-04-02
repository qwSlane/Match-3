// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using CodeBase.Infrastructure.Foundation;
using UnityEngine;
using Zenject;

namespace CodeBase.UIScripts.UIObjects
{
    public class LevelButton : AnimatedButton
    {
        [SerializeField] private string _level;
        private StateMachine _stateMachine;

        [Inject]
        public void Construct(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _button.onClick.AddListener(LoadLevel);
        }

        private void LoadLevel()
        {
            _stateMachine.Enter<LoadLevelState, string>(_level);
        }
    }
}