// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System;
using System.Collections.Generic;
using CodeBase.BoardItems;
using CodeBase.BoardItems.Token;

namespace CodeBase.Structures
{
    [Serializable]
    public class LevelConfig
    {
        public int Score;
        public int Turns;
                                            
        public int FieldColumn;
        public int FieldRows;
        public List<CellData> Field;
                                                
        public List<GoalData<ItemType>> Obstacles;
        public List<GoalData<TokenType>> Tokens;
                                        
        public LevelConfig(int score, int turns, List<CellData> field)
        {
            Score = score;
            Turns = turns;
            Field = field;
        }

        public void AddObstaclesGoal(List<GoalData<ItemType>> obstacles)
        {
            Obstacles = obstacles;
        }

        public void AddTokensGoal(List<GoalData<TokenType>> tokens)
        {
            Tokens = tokens;
        }
    }
}