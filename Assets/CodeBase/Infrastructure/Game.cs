// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using UnityEngine;
using CodeBase.Board;
using CodeBase.Board.BoardServices;
using CodeBase.BoardItems;
using CodeBase.BoardItems.Cell;
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
        private GameBoard _gameBoard;
        private ItemsChain _itemsChain;
        private MoveTask _moveTask;
        private BoardFiller _boardFiller;

        public void Awake()
        {
            _camera = Camera.main;
            _gameFactory = new GameFactory(new AssetProvider());

            _inputService.Press += Press;
            _inputService.PressUp += PressUp;

            _gameBoard = new GameBoard();
            _itemsChain = new ItemsChain(_gameBoard);

            _moveTask = new MoveTask(_gameBoard);

            _gameBoard.InitBoard(InitializeGrid());
            _boardFiller = new BoardFiller(_gameFactory, _gameBoard, _moveTask, transform);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _boardFiller.Init();
                _moveTask.InitializeItemsPaths();
            }
        }

        private void Press(Vector3 mousePos)
        {
            Ray ray = _camera.ScreenPointToRay(mousePos);
            BoardPosition screenPos = new BoardPosition(ray.origin);
            if (_gameBoard.IsOnBoard(screenPos))
            {
                IGridCell selected = _gameBoard[screenPos.PosX, screenPos.PosY];
                if (selected.IsEmpty != true && selected.Item.ItemType == ItemType.Token)
                {
                    _itemsChain.AddElement(selected);
                }
            }
        }

        private async void PressUp()
        {
            await _itemsChain.Apply();
            await _moveTask.FallDown();
            await _boardFiller.Fill();
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
                    cell.Construct(i, j, true);
                    grid[i, j] = cell;
                }
            }
            return grid;
        }
    }
}