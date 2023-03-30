// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using CodeBase.BoardItems;
using CodeBase.BoardItems.Token;
using CodeBase.EditorStructures;
using CodeBase.Services.StaticData;
using CodeBase.UIScripts;
using CodeBase.UIScripts.UIObjects;
using CodeBase.UIScripts.Windows;

namespace CodeBase.Goals
{
    public class GoalsService
    {
        private readonly HUD _hud;
        private readonly StaticDataService _dataService;

        private int _totalGoals;
        private int _currentGoals;
        private int _turns;

        private Dictionary<ItemType, UIGoal> _obstacleGoals;
        private Dictionary<TokenType, UIGoal> _tokenGoals;
        private ProgressBar _scoreBar;

        public GoalsService(HUD hud, StaticDataService dataService)
        {
            _hud = hud;
            _dataService = dataService;
            _hud.Turns.text = _turns.ToString();

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
                UIManager.Instance.Open<WinWindow>();
            }
            if (_turns == 0)
            {
                UIManager.Instance.Open<LoseWindow>();
            }
            _hud.Turns.text = _turns.ToString();
        }

        private void InitScoreGoal(LevelConfig config)
        {
            _hud.Turns.text = _turns.ToString();
            if (config.Score == 0)
                return;
            _scoreBar = _hud.InitProgressbar(config.Score);
        }

        private void InitObstacleGoals(LevelConfig config)
        {
            if (config.Obstacles.Count == 0)
                return;

            foreach (var goalData in config.Obstacles)
            {
                var goal = _hud.CreateGoal(goalData.Count, _dataService.ForItem(goalData.Type));
                _obstacleGoals[goalData.Type] = goal;
            }
            _totalGoals = _obstacleGoals.Count;
        }

        private void InitTokenGoals(LevelConfig config)
        {
            if (config.Tokens.Count == 0)
                return;

            foreach (var goalData in config.Tokens)
            {
                var goal = _hud.CreateGoal(goalData.Count, _dataService.ForToken(goalData.Type));
                _tokenGoals[goalData.Type] = goal;
            }
            _totalGoals += _tokenGoals.Count;
        }

        private void UpdateObstacleGoals(Dictionary<ItemType, int> obstacles)
        {
            if (_obstacleGoals.Count == 0)
                return;

            foreach (var goal in _obstacleGoals)
            {
                var count = (obstacles.TryGetValue(goal.Key, out int t)) ? t : 0;

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
                var count = (dataTokens.TryGetValue(goal.Key, out var t)) ? t : 0;

                if (_tokenGoals[goal.Key].CountUpdate(count))
                {
                    _currentGoals -= 1;
                }
            }
        }
    }
}