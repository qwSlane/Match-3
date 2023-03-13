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
        public LevelEditorSettings EditorSettings => _editorSettings;

        private readonly LevelEditorSettings _editorSettings;
        private readonly LevelCreator _levelCreator;
        private bool _isScore;

        public ScoreMenu(LevelCreator levelCreator)
        {
            _levelCreator = levelCreator;
            _editorSettings = new LevelEditorSettings();
            _isScore = false;
        }

        public void LeftMenu()
        {
            string turns;

            GUILayout.Space(20);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Turns count:");
            turns = Extentions.ValueOrEmpty(_editorSettings.Turns);
            turns = GUILayout.TextField(turns,4);
            _editorSettings.Turns = 
                (Int32.TryParse(turns, out int t)) ? t : 0;
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
            string score = "";
            GUILayout.BeginHorizontal();
            GUILayout.Label("Score");
            score = Extentions.ValueOrEmpty(_editorSettings.Score);
            score = GUILayout.TextField(score, 13);
            _editorSettings.Score =
                (Int32.TryParse(score, out int t)) ? t : 0;

            if (GUILayout.Button("x"))
            {
                _isScore = false;
                _editorSettings.Score = 0;
            }
            GUILayout.EndHorizontal();
        }

        private void TokenGoals()
        {
            if (_editorSettings.TokenGoal.Count == 0)
                return;

            string count;
            GUILayout.Space(10);
            _levelCreator.AlignedLabel("Tokens");
            foreach (KeyValuePair<TokenType, int> token in _editorSettings.TokenGoal.ToDictionary(x => x.Key, x => x.Value))
            {
                count = Extentions.ValueOrEmpty(token.Value);
                GUILayout.BeginHorizontal();
                GUILayout.Label($"{token.Key}:");
                count = GUILayout.TextField(count, 5);
                _editorSettings.TokenGoal[token.Key] = (Int32.TryParse(count, out int t)) ? t : 0;
                if (GUILayout.Button("x"))
                {
                    _isScore = false;
                    _editorSettings.RemoveTokenGoal(token.Key);
                }
                GUILayout.EndHorizontal();
            }
        }

        private void ObstacleGoals()
        {
            if (_editorSettings.ObstacleGoal.Count == 0)
                return;

            GUILayout.Space(10);
            _levelCreator.AlignedLabel("Remove obstacles");
            Recount();
            foreach (KeyValuePair<ItemType, int> obstacle in _editorSettings.ObstacleGoal.ToDictionary(x => x.Key, x => x.Value))
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label($"{obstacle.Key} : {obstacle.Value}");
                if (GUILayout.Button("x"))
                {
                    _isScore = false;
                    _editorSettings.RemoveObstacleGoal(obstacle.Key);
                }
                GUILayout.EndHorizontal();
            }
        }

        private void Recount()
        {
            IEnumerable<ItemType> types = _editorSettings.ObstacleGoal.Keys.ToList();
            foreach (ItemType key in types)
            {
                _editorSettings.ObstacleGoal[key] = _levelCreator.GetObstacleCount(key);
            }
        }

        private void ScoreGoal()
        {
            _isScore = true;
        }

        private void CollectGoal(object userdata)
        {
            TokenType tokenType = (TokenType)userdata;
            _editorSettings.TokenGoal[tokenType] = 0;
        }

        private void RemoveGoal(object userdata)
        {
            ItemType type = (ItemType)(userdata);
            _editorSettings.ObstacleGoal[type] = _levelCreator.GetObstacleCount(type);
        }
    }
}