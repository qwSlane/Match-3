// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System;
using System.Collections.Generic;
using CodeBase.EditorStructures;
using CodeBase.Infrastructure.Foundation.States;
using CodeBase.Infrastructure.Foundation.States.Intefaces;

namespace CodeBase.Infrastructure.Foundation
{
    public class StateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;
        private IExitableState _currentState;

        public IExitableState CurrentState => _currentState;

        private LevelConfig _currentLevel;

        public LevelConfig LevelData
        {
            get => _currentState is GameState ? _currentLevel : null;
            set
            {
                if (_currentState is LoadLevelState)
                {
                    _currentLevel = value;
                }
            }
        }

        public StateMachine()
        {
            _states = new Dictionary<Type, IExitableState>()
            {
                [typeof(SceneLoadState)] = new SceneLoadState(this),
                [typeof(LoadLevelState)] = new LoadLevelState(this),
                [typeof(GameState)] = new GameState(),
                [typeof(RestartLevelState)] = new RestartLevelState(this),
            };
        }

        public void Enter<TState>() where TState : class, IState
        {
            IState state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadState<TPayload>
        {
            TState state = ChangeState<TState>();
            state.Enter(payload);
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _currentState?.Exit();
            TState state = GetState<TState>();
            _currentState = state;
            return state;
        }

        private TState GetState<TState>() where TState : class, IExitableState
        {
            return _states[typeof(TState)] as TState;
        }
    }
}