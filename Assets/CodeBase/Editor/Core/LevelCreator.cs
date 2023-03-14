// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System.IO;
using UnityEditor;
using UnityEngine;
using CodeBase.BoardItems;
using CodeBase.EditorStructures;

namespace CodeBase.Editor.Core
{
    public class LevelCreator : EditorWindow
    {
        private const int FieldWidth = 7;
        private const int FieldHeight = 11;
        private const string LevelTools = "Prefabs/LevelCreator/LevelTools";

        private Vector2 _offset;
        private Vector2 _drag;

        private Node[,] _gameField;

        private Vector2 _nodePos;

        private LevelTools _tools;
        private ToolAsset _current;
        private GUIStyle _buttonStyle;

        private ScoreMenu _scoreMenu;

        [MenuItem("Tools/Level creator")]
        private static void OpenWindow()
        {
            LevelCreator window = GetWindow<LevelCreator>();
            window.titleContent = new GUIContent("Level creator");
        }

        private void OnEnable()
        {
            _offset = new Vector2(200, 100);
            _scoreMenu = new ScoreMenu(this);

            SetupTools();
            SetupGameField();
        }

        private void SetupTools()
        {
            _tools = Resources.Load<LevelTools>(LevelTools);

            for (int i = 0; i < _tools.FieldTools.Length; i++)
            {
                _tools.FieldTools[i].Style = new GUIStyle();
                _tools.FieldTools[i].Style.normal.background = _tools.FieldTools[i].Icon;
            }

            _current = _tools.FieldTools[1];
            _buttonStyle = EditorStyles.toolbarButton;
            _buttonStyle.fixedHeight = 50;
            _buttonStyle.fixedWidth = 50;
        }

        private void OnGUI()
        {
            DrawField();
            DrawRightMenuBar();
            DrawLeftMenuBar();
            if (GUI.changed)
            {
                Repaint();
            }
        }

        private void DrawField()
        {
            DrawGrid();
            ProcessNodes(Event.current);
            ProcessGrid(Event.current);
            DrawNodes();
        }

        private void DrawLeftMenuBar()
        {
            Rect rect = new Rect(0, 0, 170, position.height);
            GUIStyle box = "Box";
            GUILayout.BeginArea(rect, box);
            AlignedLabel("Level Properties");
            _scoreMenu.LeftMenu();

            if (GUILayout.Button("Create"))
            {
                LevelSerializer s = new LevelSerializer();

                LevelEditorSettings editorSettings = _scoreMenu.EditorSettings;
                string json = s.SerializeField(_gameField, editorSettings);

                File.WriteAllText(Application.dataPath + "/t1.json", json);
                AssetDatabase.Refresh();
            }
            GUILayout.EndArea();
        }

        private void DrawRightMenuBar()
        {
            Rect rect = new Rect(position.width - 70, 0, 70, position.height);
            GUIStyle box = "Box";
            GUILayout.BeginArea(rect, box);

            AlignedLabel("Tools");

            foreach (ToolAsset tool in _tools.FieldTools)
            {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Toggle((_current == tool), new GUIContent(tool.Icon, tool.Hint), _buttonStyle,
                        GUILayout.Width(50), GUILayout.Height(50)))
                    _current = tool;
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }
            GUILayout.EndArea();
        }

        private void ProcessNodes(Event e)
        {
            int row = (int)((e.mousePosition.x - _offset.x) / 30);
            int column = FieldHeight - (int)((e.mousePosition.y - _offset.y) / 30);

            if (row < 0 || row >= FieldWidth || column < 0 || column >= FieldHeight)
            {
                return;
            }

            if (e.type == EventType.MouseDown)
            {
                _gameField[row, column].SetStyle(_current.Style, _current.Type);
                GUI.changed = true;
            }
            if (e.type == EventType.MouseDrag)
            {
                _gameField[row, column].SetStyle(_current.Style, _current.Type);
                GUI.changed = true;

                e.Use();
            }
        }

        private void ProcessGrid(Event e)
        {
            _drag = Vector2.zero;
            switch (e.type)
            {
                case EventType.MouseDrag:
                    if (e.button == 0)
                    {
                        OnMouseDrag(e.delta);
                    }
                    break;
            }
        }

        private void OnMouseDrag(Vector2 delta)
        {
            _drag = delta;
            for (int i = 0; i < FieldWidth; i++)
            {
                for (int j = 0; j < FieldHeight; j++)
                {
                    _gameField[i, j].Drag(delta);
                }
            }
            GUI.changed = true;
        }

        private void DrawNodes()
        {
            for (int i = 0; i < FieldWidth; i++)
            {
                for (int j = 0; j < FieldHeight; j++)
                {
                    _gameField[i, j].Draw();
                }
            }
        }

        private void DrawGrid()
        {
            int width = Mathf.CeilToInt(position.width / 30);
            int height = Mathf.CeilToInt(position.height / 30);

            Handles.BeginGUI();
            Handles.color = new Color(.5f, .5f, .5f, .2f);
            _offset += _drag;
            Vector3 offset = new Vector3(_offset.x % 30, _offset.y % 30, 0);
            for (int i = 0; i < width; i++)
            {
                Handles.DrawLine(new Vector3(30 * i, -30, 0) + offset,
                    new Vector3(30 * i, position.height, 0) + offset);
            }
            for (int i = 0; i < height; i++)
            {
                Handles.DrawLine(new Vector3(-30, 30 * i, 0) + offset,
                    new Vector3(position.width, 30 * i, 0) + offset);
            }

            Handles.color = Color.white;
            Handles.EndGUI();
        }

        private void SetupGameField()
        {
            _gameField = new Node[FieldWidth, FieldHeight];

            for (int i = 0; i < FieldWidth; i++)
            {
                for (int j = 0; j < FieldHeight; j++)
                {
                    _nodePos.Set(i * 30 + _offset.x, 330 - (30 * j) + _offset.y);
                    _gameField[i, j] = new Node(_nodePos, 30, 30, _current.Style);
                }
            }
        }

        public void AlignedLabel(string text)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(text);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        public int GetObstacleCount(ItemType type)
        {
            NodeType nodeType = NodeType.Empty;
            int count = 0;

            switch (type)
            {
                case ItemType.Ice:
                    nodeType = NodeType.Ice;
                    break;
                case ItemType.Stone:
                    nodeType = NodeType.Stone;
                    break;
                case ItemType.ReinforcedStone:
                    nodeType = NodeType.ReinforcedStone;
                    break;
            }

            foreach (Node node in _gameField)
            {
                if (node.Type == nodeType)
                {
                    count++;
                }
            }

            return count;
        }
    }
}