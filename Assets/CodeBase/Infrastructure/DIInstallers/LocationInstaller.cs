// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
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
using CodeBase.UIScripts.Windows;

namespace CodeBase.Infrastructure.DIInstallers
{
    public class LocationInstaller : MonoInstaller
    {
        [SerializeField] private Transform _boardParent;
        [SerializeField] private Transform _canvas;

        [SerializeField] private HUD _kernel;

        [SerializeField] private GameObject _settingsWindow;
        [SerializeField] private GameObject _winWindow;
        [SerializeField] private GameObject _loseWindow;

        public override void InstallBindings()
        {
            BindAssetProvider();
            BindGameFactory();
            BindStaticDataService();

            BindBoardServices();
            BindGoalsService();

            BindUIWindows();
        }

        private void BindUIWindows()
        {
            var settingsWindow = Container.InstantiatePrefabForComponent<SettingsWindow>(_settingsWindow, _canvas);
            var loseWindow = Container.InstantiatePrefabForComponent<LoseWindow>(_loseWindow, _canvas);
            var winWindow = Container.InstantiatePrefabForComponent<WinWindow>(_winWindow, _canvas);

            List<Window> windows = new List<Window>
            {
                settingsWindow,
                loseWindow,
                winWindow
            };

            Container
                .Bind<List<Window>>()
                .FromInstance(windows)
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