// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using Newtonsoft.Json;
using CodeBase.BoardItems;
using CodeBase.BoardItems.Token;
using CodeBase.Editor.Core;
using CodeBase.Structures;

namespace CodeBase.Editor
{
    public class LevelSerializer
    {
        public string SerializeField(Node[,] field, LevelEditorSettings editorSettings)
        {
            List<CellData> cellsData = new List<CellData>();

            for (int i = 0; i < field.GetLength(0); i++)
            {
                for (int j = 0; j < field.GetLength(1); j++)
                {
                    cellsData.Add(new CellData(
                        new BoardPosition(i, j), field[i, j].Type));
                }
            }
            
            LevelConfig config = new LevelConfig(editorSettings.Score, editorSettings.Turns, cellsData);

            config.FieldColumn = field.GetLength(0);
            config.FieldRows = field.GetLength(1);

            config.AddObstaclesGoal(ObstacleGoals(editorSettings));
            config.AddTokensGoal(TokenGoals(editorSettings));
            
            return JsonConvert.SerializeObject(config, Formatting.Indented);
           
        }

        private static List<GoalData<TokenType>> TokenGoals(LevelEditorSettings editorSettings)
        {
            List<GoalData<TokenType>> tokens = new List<GoalData<TokenType>>();

            foreach (KeyValuePair<TokenType, int> pair in editorSettings.TokenGoal)
            {
                tokens.Add(new GoalData<TokenType>(pair.Key, pair.Value));
            }
            return tokens;
        }

        private static List<GoalData<ItemType>> ObstacleGoals(LevelEditorSettings editorSettings)
        {
            List<GoalData<ItemType>> obstacles = new List<GoalData<ItemType>>();
            foreach (KeyValuePair<ItemType, int> pair in editorSettings.ObstacleGoal)
            {
                obstacles.Add(new GoalData<ItemType>(pair.Key, pair.Value));
            }
            return obstacles;
        }
    }
}