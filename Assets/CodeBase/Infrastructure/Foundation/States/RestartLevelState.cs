// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using UnityEngine.SceneManagement;
using CodeBase.Infrastructure.Foundation.States.Intefaces;

namespace CodeBase.Infrastructure.Foundation.States
{
    public class RestartLevelState : IState
    {
        private readonly StateMachine _stateMachine;

        public RestartLevelState(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
        }

        public void Exit()
        {
            _stateMachine.Enter<GameState>();
        }
    }
}