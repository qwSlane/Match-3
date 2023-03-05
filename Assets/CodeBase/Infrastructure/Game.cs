// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using UnityEngine;
using CodeBase.Board;
using CodeBase.Board.BoardServices;
using CodeBase.Board.BoardServices.Score;
using CodeBase.BoardItems;
using CodeBase.BoardItems.Cell;
using CodeBase.Goals;
using CodeBase.TaskRunner;
using CodeBase.Services;
using CodeBase.Services.AssetService;

namespace CodeBase.Infrastructure
{
    public class Game : MonoBehaviour
    {
        private const string PrefabA = "Prefabs/GameBoard/Cell_0";
        private const string PrefabB = "Prefabs/GameBoard/Cell_1";

        [SerializeField] private Camera _camera;
        [SerializeField] private InputService _inputService;

        private GameFactory _gameFactory;
        private GameBoard _gameBoard;
        private ItemsChain _itemsChain;
        private ItemCrusher _itemCrusher;
        private BoardFiller _boardFiller;
        private ScoreCounter _scoreCounter;
        private GoalsManager _goalsManager;

        public void Awake()
        {
            _camera = Camera.main;
            IAssetProvider assetProvider = new AssetProvider();
            _gameFactory = new GameFactory(assetProvider);

            _inputService.Press += Press;
            _inputService.PressUp += PressUp;

            _gameBoard = new GameBoard();
            _itemsChain = new ItemsChain();

            _itemCrusher = new ItemCrusher(_gameBoard);

            _gameBoard.InitBoard(InitializeGrid());
            _boardFiller = new BoardFiller(_gameFactory, _gameBoard, transform, assetProvider);

            _scoreCounter = new ScoreCounter(_gameFactory, _itemsChain);

            _goalsManager = new GoalsManager();

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _boardFiller.Init();
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
            if (_itemsChain.Count >= 3)
            {
                _itemCrusher.Crush(_itemsChain);

                await _scoreCounter.Count(_itemCrusher.ToCrush);
                _boardFiller.CreateModifier(_itemsChain.Count);
                
                _goalsManager.Recieve(_scoreCounter.TurnData);
                
                _itemCrusher.Clean();
                _itemsChain.Clean();
                
                await _boardFiller.Fill();
            }
            _itemsChain.Deselect();
        }

        private Cell[,] InitializeGrid()
        {
            Cell[,] grid = new Cell[7, 11];
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    string path = ((i + j & 1) == 0) ? PrefabA : PrefabB;
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