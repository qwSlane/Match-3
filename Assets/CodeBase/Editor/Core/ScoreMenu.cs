// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using CodeBase.BoardItems;
using CodeBase.BoardItems.Token;

namespace CodeBase.Editor.Core
{
    public class ScoreMenu
    {
        private string _score = "10000";
        private string _turns = "5";

        private readonly LevelGoals _goals;
        private readonly LevelCreator _levelCreator;
        private bool _isScore;

        public ScoreMenu(LevelCreator levelCreator)
        {
            _levelCreator = levelCreator;
            _goals = new LevelGoals();
            _isScore = false;
        }

        public void LeftMenu()
        {
            GUILayout.Space(20);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Turns count:");
            _turns = GUILayout.TextField(_turns);
            GUILayout.EndHorizontal();

            if (GUILayout.Button("Add goal"))
            {
                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent("Score"), false, ScoreGoal);
                menu.AddItem(new GUIContent("Remove/Ice"), false, RemoveGoal, ItemType.Ice);
                menu.AddItem(new GUIContent("Remove/Stone"), false, RemoveGoal, ItemType.Stone);
                menu.AddItem(new GUIContent("Remove/ReinforcedStone"), false, RemoveGoal, ItemType.ReinforcedStone);
                menu.AddItem(new GUIContent("Collect/Red"), false, CollectGoal, TokenType.Red);
                menu.AddItem(new GUIContent("Collect/Blue"), false, CollectGoal, TokenType.Blue);
                menu.AddItem(new GUIContent("Collect/Yellow"), false, CollectGoal, TokenType.Yellow);
                menu.AddItem(new GUIContent("Collect/Pink"), false, CollectGoal, TokenType.Pink);
                menu.AddItem(new GUIContent("Collect/Green"), false, CollectGoal, TokenType.Green);

                menu.ShowAsContext();
            }

            if (_isScore)
            {
                ScoreGUI();
            }
            ObstacleGoals();
            TokenGoals();
        }

        private void ScoreGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Score");
            _score = GUILayout.TextField(_score, 13);
            _goals.AddScoreGoal(Int32.Parse(_score));
            if (GUILayout.Button("x"))
            {
                _isScore = false;
                _goals.AddScoreGoal(0);
            }
            GUILayout.EndHorizontal();
        }

        private void ObstacleGoals()
        {
            if (_goals.ObstacleGoal.Count == 0)
                return;
            GUILayout.Space(50);
            _levelCreator.AlignedLabel("Remove obstacles");
            Recount();
            foreach (KeyValuePair<ItemType, int> obstacle in _goals.ObstacleGoal.ToDictionary(x => x.Key, x => x.Value))
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label($"{obstacle.Key} : {obstacle.Value}");
                if (GUILayout.Button("x"))
                {
                    _isScore = false;
                    _goals.RemoveObstacleGoal(obstacle.Key);
                }
                GUILayout.EndHorizontal();
            }
        }

        private void TokenGoals()
        {
            if (_goals.TokenGoal.Count == 0)
                return;
            
            string count = " ";
            GUILayout.Space(50);
            _levelCreator.AlignedLabel("Tokens");
            foreach (KeyValuePair<TokenType, int> token in _goals.TokenGoal.ToDictionary(x => x.Key, x => x.Value))
            {
                count = (string.IsNullOrEmpty(token.Value.ToString()) || token.Value == 0)
                    ? count
                    : token.Value.ToString();
                GUILayout.BeginHorizontal();
                GUILayout.Label($"{token.Key}:");
                count = GUILayout.TextField(count, 5);
                _goals.TokenGoal[token.Key] = (Int32.TryParse(count, out int t)) ? t : 0;
                if (GUILayout.Button("x"))
                {
                    _isScore = false;
                    _goals.RemoveTokenGoal(token.Key);
                }
                GUILayout.EndHorizontal();
            }
        }

        private void Recount()
        {
            IEnumerable<ItemType> types = _goals.ObstacleGoal.Keys.ToList();
            foreach (ItemType key in types)
            {
                _goals.ObstacleGoal[key] = _levelCreator.GetObstacleCount(key);
            }
        }

        private void ScoreGoal()
        {
            _isScore = true;
        }

        private void CollectGoal(object userdata)
        {
            TokenType tokenType = (TokenType)userdata;
            _goals.TokenGoal[tokenType] = 0;
        }

        private void RemoveGoal(object userdata)
        {
            ItemType type = (ItemType)(userdata);
            _goals.ObstacleGoal[type] = _levelCreator.GetObstacleCount(type);
        }
    }
}