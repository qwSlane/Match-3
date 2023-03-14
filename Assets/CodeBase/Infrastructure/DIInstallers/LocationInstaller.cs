// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using UnityEngine;
using Zenject;
using CodeBase.Board;
using CodeBase.Board.BoardKernel;
using CodeBase.Board.BoardKernel.Score;
using CodeBase.Goals;
using CodeBase.Services;
using CodeBase.Services.AssetService;
using CodeBase.Services.StaticData;
using CodeBase.UIScripts;
using CodeBase.UIScripts.Services;

namespace CodeBase.Infrastructure.DIInstallers
{
    public class LocationInstaller : MonoInstaller
    {
        [SerializeField] private Transform _boardParent;
        [SerializeField] private UIKernel _kernel;

        public override void InstallBindings()
        {
            BindAssetProvider();
            BindGameFactory();
            BindStaticDataService();

            BindBoardServices();

            BindUIFactory();
            BindGoalsService();
        }

        private void BindUIFactory()
        {
            Container
                .Bind<UIFactory>()
                .AsSingle();
        }

        private void BindGoalsService()
        {
            Container
                .Bind<GoalsService>()
                .AsSingle()
                .WithArguments(_kernel);
        }

        private void BindBoardServices()
        {
            Container
                .Bind<GameBoard>()
                .AsSingle()
                .WithArguments(_boardParent);

            Container
                .Bind<ItemsChain>()
                .AsSingle();

            Container
                .Bind<ScoreSpawner>()
                .AsSingle();

            Container
                .Bind<ItemCrusher>()
                .AsSingle();

            Container
                .Bind<BoardFiller>()
                .AsSingle();
        }

        private void BindStaticDataService()
        {
            Container
                .Bind<StaticDataService>()
                .AsSingle();
        }

        private void BindAssetProvider()
        {
            Container
                .Bind<IAssetProvider>()
                .To<AssetProvider>()
                .AsSingle();
        }

        private void BindGameFactory()
        {
            Container
                .Bind<GameFactory>()
                .AsSingle();
        }
    }
}