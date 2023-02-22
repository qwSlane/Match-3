// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System;
using CodeBase.Grid;
using CodeBase.Items;
using UnityEngine;
using CodeBase.Services;
using CodeBase.Services.AssetService;
using UnityEditor;

namespace CodeBase.Infrastructure
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private InputService _inputService;

        private const int ItemsLayerMask = 1 << 7;
        private GameFactory _gameFactory;
        private GameBoard _gameBoard;
        private ItemsChain _itemsChain;

        public void Awake()
        {
            _camera = Camera.main;
            _gameFactory = new GameFactory(new AssetProvider());

            _inputService.Press += Press;
            _inputService.PressUp += PressUp;

            _gameBoard = new GameBoard();
            _itemsChain = new ItemsChain();

            _gameBoard.InitBoard(InitializeGrid());
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _gameBoard.Fill(_gameFactory);
            }
        }

        private void Press(Vector3 mousePos)
        {
            Ray ray = _camera.ScreenPointToRay(mousePos);
            RaycastHit hit;
            BoardPosition screenPos = new BoardPosition(ray.origin);
            if (_gameBoard.IsOnBoard(screenPos))
            {
                Item selected = _gameBoard.Cell(screenPos.PosX, screenPos.PosY).Item;
                _itemsChain.AddElement(selected);
            }
        }

        private void PressUp()
        {
           _itemsChain.Apply();
        }

        private Cell[,] InitializeGrid()
        {
            Cell[,] grid = new Cell[7, 11];
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    string path = ((i + j & 1) == 0) ? "Cell_0" : "Cell_1";
                    Vector3 position = new Vector3(i, j, 1);
                    Cell cell = _gameFactory.Create(path, position, transform);
                    cell.Construct(i, j);
                    grid[i, j] = cell;
                }
            }

            return grid;
        }
    }
}