// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using CodeBase.Board;
using CodeBase.Board.BoardServices;
using CodeBase.Board.BoardServices.Score;
using CodeBase.BoardItems;
using CodeBase.BoardItems.Cell;
using CodeBase.Goals;
using CodeBase.Services;
using CodeBase.Services.AssetService;
using CodeBase.Structures;
using CodeBase.UIScripts;
using CodeBase.UIScripts.Services;
using CodeBase.UIScripts.Services.StaticData;
using Newtonsoft.Json;
using UnityEngine.Serialization;
using Input = UnityEngine.Input;

namespace CodeBase.Infrastructure
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private InputService _inputService;

        [FormerlySerializedAs("_mediator")] [SerializeField]
        private UIKernel kernel;

        private string path = @"Assets/t1.json";

        private GameFactory _gameFactory;
        private GameBoard _gameBoard;

        private ItemsChain _itemsChain;
        private ItemCrusher _itemCrusher;
        private BoardFiller _boardFiller;

        private ScoreCounter _scoreCounter;
        private GoalsKernel _goalsKernel;

        public void Awake()
        {
            _camera = Camera.main;
            IAssetProvider assetProvider = new AssetProvider();
            _gameFactory = new GameFactory(assetProvider);

            _inputService.Press += Press;
            _inputService.PressUp += PressUp;

            StaticDataService dataService = new StaticDataService();

            LevelConfig config = JsonConvert.DeserializeObject<LevelConfig>(File.ReadAllText(path));

            _gameBoard = new GameBoard(transform, _gameFactory, dataService);
            _itemsChain = new ItemsChain();

            _itemCrusher = new ItemCrusher(_gameBoard);

            _gameBoard.InitCells(config);
            _boardFiller = new BoardFiller(_gameFactory, _gameBoard);

            _scoreCounter = new ScoreCounter(_gameFactory, _itemsChain);

            _goalsKernel = new GoalsKernel(config, kernel, dataService, new UIFactory(dataService));
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _boardFiller.InitFill();
            }
        }

        private void Press(Vector3 mousePos)
        {
            Ray ray = _camera.ScreenPointToRay(mousePos);
            BoardPosition screenPos = new BoardPosition(ray.origin);
            if (_gameBoard.IsOnBoard(screenPos))
            {
                IGridCell selected = _gameBoard[screenPos.PosX, screenPos.PosY];
                if (selected.IsStorable && selected.IsEmpty != true && selected.Item.ItemType == ItemType.Token)
                {
                    _itemsChain.AddElement(selected);
                }
            }
        }

        private async void PressUp()
        {
            if (_itemsChain.Count >= 3 || _itemsChain.IsModifier())
            {
                await Turn();
            }

            _itemsChain.Deselect();
        }

        private async Task Turn()
        {
            _itemCrusher.Crush(_itemsChain);

            await _scoreCounter.Count(_itemCrusher.ToCrush);
            _boardFiller.CreateModifier(_itemsChain.Count);
            _goalsKernel.Recieve(_scoreCounter.TurnData);

            _itemCrusher.Clean();
            _itemsChain.Clean();

            await _boardFiller.Fill();
        }
    }
}