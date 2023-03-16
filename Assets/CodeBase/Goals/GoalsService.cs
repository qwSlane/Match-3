// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System;
using System.Collections.Generic;
using CodeBase.BoardItems;
using CodeBase.BoardItems.Token;
using CodeBase.EditorStructures;
using CodeBase.Services.StaticData;
using CodeBase.UIScripts;
using CodeBase.UIScripts.Services;
using CodeBase.UIScripts.UIObjects;

namespace CodeBase.Goals
{
    public class GoalsService
    {
        public Action OnFinish;
        
        private readonly UIKernel _kernel;
        private readonly UIFactory _factory;
        private readonly StaticDataService _dataService;

        private int _totalGoals;
        private int _currentGoals;
        private int _turns;

        private Dictionary<ItemType, UIGoal> _obstacleGoals;
        private Dictionary<TokenType, UIGoal> _tokenGoals;
        private ProgressBar _scoreBar;

        public GoalsService(UIKernel kernel, StaticDataService dataService, UIFactory factory)
        {
            _kernel = kernel;
            _dataService = dataService;
            _factory = factory;
            _kernel.Turns.text = _turns.ToString();

            _obstacleGoals = new Dictionary<ItemType, UIGoal>();
            _tokenGoals = new Dictionary<TokenType, UIGoal>();
        }

        public void Init(LevelConfig config)
        {
            _turns = config.Turns;
            InitScoreGoal(config);

            InitObstacleGoals(config);
            InitTokenGoals(config);
        }

        public void Receive(TurnData data)
        {
            _currentGoals = _totalGoals;

            UpdateObstacleGoals(data.Obstacles);
            UpdateTokenGoals(data.Tokens);
            _scoreBar?.UpdateScore(data.Score);

            CheckEnd();
        }

        private void CheckEnd()
        {
            _turns -= 1;
            bool score = (_scoreBar == null) || _scoreBar.IsAchived();

            if (_currentGoals == 0 && score)
            {
                Popup winPopup = _factory.CreateWinPopup(_kernel.Canvas);
                _kernel.DisableInput();
                winPopup.Construct(OnFinish);
            }
            if (_turns == 0)
            {
                Popup losePopUp = _factory.CreateLosePopUp(_kernel.Canvas);
                _kernel.DisableInput();
                losePopUp.Construct(OnFinish);
            }
            _kernel.Turns.text = _turns.ToString();
        }

        private void InitScoreGoal(LevelConfig config)
        {
            _kernel.Turns.text = _turns.ToString();
            if (config.Score == 0)
                return;
            _scoreBar = _factory.CreateProgressBar(_kernel.GoalsContainer);
            _scoreBar.Construct(config.Score);
        }

        private void InitTokenGoals(LevelConfig config)
        {
            if (config.Tokens.Count == 0)
                return;

            foreach (var goalData in config.Tokens)
            {
                UIGoal goal = _factory.CreateGoal(_kernel.GoalsParent);
                goal.Image.sprite = _dataService.ForToken(goalData.Type);
                goal.Construct(goalData.Count);
                _tokenGoals[goalData.Type] = goal;
            }
            _totalGoals = _tokenGoals.Count;
        }

        private void InitObstacleGoals(LevelConfig config)
        {
            if (config.Obstacles.Count == 0)
                return;

            foreach (var goalData in config.Obstacles)
            {
                UIGoal goal = _factory.CreateGoal(_kernel.GoalsParent);
                goal.Image.sprite = _dataService.ForItem(goalData.Type);
                goal.Construct(goalData.Count);
                _obstacleGoals[goalData.Type] = goal;
            }
            _totalGoals = _obstacleGoals.Count;
        }

        private void UpdateObstacleGoals(Dictionary<ItemType, int> obstacles)
        {
            if (_obstacleGoals.Count == 0)
                return;

            foreach (var goal in _obstacleGoals)
            {
                int count = (obstacles.TryGetValue(goal.Key, out int t)) ? t : 0;

                if (_obstacleGoals[goal.Key].CountUpdate(count))
                {
                    _currentGoals -= 1;
                }
            }
        }

        private void UpdateTokenGoals(Dictionary<TokenType, int> dataTokens)
        {
            if (_obstacleGoals.Count == 0)
                return;

            foreach (var goal in _tokenGoals)
            {
                int count = (dataTokens.TryGetValue(goal.Key, out int t)) ? t : 0;

                if (_tokenGoals[goal.Key].CountUpdate(count))
                {
                    _currentGoals -= 1;
                }
            }
        }
    }
}