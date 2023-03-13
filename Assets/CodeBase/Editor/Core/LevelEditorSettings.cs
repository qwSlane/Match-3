// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using CodeBase.BoardItems;
using CodeBase.BoardItems.Token;

namespace CodeBase.Editor.Core
{
    public class LevelEditorSettings
    {
        public Dictionary<ItemType, int> ObstacleGoal => _obstacles;

        public Dictionary<TokenType, int> TokenGoal => _tokens;

        public int Score { get; set; }

        public int Turns { get; set; }

        private Dictionary<ItemType, int> _obstacles;
        private Dictionary<TokenType, int> _tokens;

        public LevelEditorSettings()
        {
            _obstacles = new Dictionary<ItemType, int>();
            _tokens = new Dictionary<TokenType, int>();
        }

        public void AddObstacles(ItemType type, int count)
        {
            _obstacles[type] = count;
        }

        public void AddTokens(TokenType type, int count)
        {
            _tokens[type] = count;
        }

        public void RemoveObstacleGoal(ItemType obstacleKey)
        {
            _obstacles.Remove(obstacleKey);
        }

        public void RemoveTokenGoal(TokenType token)
        {
            _tokens.Remove(token);
        }
    }
}