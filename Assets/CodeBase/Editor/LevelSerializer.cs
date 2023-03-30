// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using Newtonsoft.Json;
using CodeBase.BoardItems;
using CodeBase.BoardItems.Token;
using CodeBase.Editor.Core;
using CodeBase.EditorStructures;

namespace CodeBase.Editor
{
    public class LevelSerializer
    {
        public string SerializeField(Node[,] field, LevelEditorSettings editorSettings)
        {
            var cellsData = new List<CellData>();

            for (var i = 0; i < field.GetLength(0); i++)
            {
                for (var j = 0; j < field.GetLength(1); j++)
                {
                    cellsData.Add(new CellData(
                        new BoardPosition(i, j), field[i, j].Type));
                }
            }
            
            var config = new LevelConfig(editorSettings.Score, editorSettings.Turns, cellsData)
            {
                FieldColumn = field.GetLength(0),
                FieldRows = field.GetLength(1)
            };

            config.AddObstaclesGoal(ObstacleGoals(editorSettings));
            config.AddTokensGoal(TokenGoals(editorSettings));
            
            return JsonConvert.SerializeObject(config, Formatting.Indented);
           
        }

        private static List<GoalData<TokenType>> TokenGoals(LevelEditorSettings editorSettings)
        {
            var tokens = new List<GoalData<TokenType>>();

            foreach (var pair in editorSettings.TokenGoal)
            {
                tokens.Add(new GoalData<TokenType>(pair.Key, pair.Value));
            }
            return tokens;
        }

        private static List<GoalData<ItemType>> ObstacleGoals(LevelEditorSettings editorSettings)
        {
            var obstacles = new List<GoalData<ItemType>>();
            foreach (var pair in editorSettings.ObstacleGoal)
            {
                obstacles.Add(new GoalData<ItemType>(pair.Key, pair.Value));
            }
            return obstacles;
        }
    }
}