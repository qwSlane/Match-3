// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using Zenject;
using CodeBase.Infrastructure.Foundation;

namespace CodeBase.Infrastructure.DIInstallers
{
    public class GlobalInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindStateMachine();
        }

        private void BindStateMachine()
        {
            Container.Bind<StateMachine>()
                .AsSingle();
        }
    }
}