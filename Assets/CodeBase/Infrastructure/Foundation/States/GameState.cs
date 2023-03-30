// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using CodeBase.Infrastructure.Foundation.States.Intefaces;

namespace CodeBase.Infrastructure.Foundation.States
{
    public class GameState: IState
    {
        private IState _stateImplementation;
        
        public void Exit() {}

        public void Enter() { }
    }
}