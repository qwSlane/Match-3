// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using UnityEngine;
using CodeBase.GameBoard;
using CodeBase.Items;
using CodeBase.TaskRunner;
using CodeBase.Services;
using CodeBase.Services.AssetService;

namespace CodeBase.Infrastructure
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private InputService _inputService;

        private GameFactory _gameFactory;
        private GameBoard.GameBoard _gameBoard;
        private ItemsChain _itemsChain;
        private MoveTask _moveTask;

        public void Awake()
        {
            _camera = Camera.main;
            _gameFactory = new GameFactory(new AssetProvider());

            _inputService.Press += Press;
            _inputService.PressUp += PressUp;

            _gameBoard = new GameBoard.GameBoard();
            _itemsChain = new ItemsChain();

            _moveTask = new MoveTask(_gameBoard);

            _gameBoard.InitBoard(InitializeGrid());
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _gameBoard.Fill(_gameFactory, transform);
            }
        }

        private void Press(Vector3 mousePos)
        {
            Ray ray = _camera.ScreenPointToRay(mousePos);
            BoardPosition screenPos = new BoardPosition(ray.origin);
            if (_gameBoard.IsOnBoard(screenPos))
            {
                IGriddable selected = _gameBoard[screenPos.PosX, screenPos.PosY];
                _itemsChain.AddElement(selected);
            }
        }

        private async void PressUp()
        {
            await _itemsChain.Apply();
            _moveTask.CreatePath();
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