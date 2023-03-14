// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using Newtonsoft.Json;
using CodeBase.Board;
using CodeBase.Board.BoardKernel;
using CodeBase.BoardItems;
using CodeBase.BoardItems.Cell;
using CodeBase.EditorStructures;
using CodeBase.Goals;
using CodeBase.Services;

namespace CodeBase.Infrastructure
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private InputService _inputService;

        private string path = @"Assets/t1.json";
        private LevelConfig _config;

        private ItemsChain _itemsChain;
        private ItemCrusher _itemCrusher;
        private BoardFiller _boardFiller;

        private GameBoard _gameBoard;
        private GoalsService _goalsService;

        [Inject]
        public void Initialize(GameBoard board, ItemsChain chain, ItemCrusher crusher,
            BoardFiller filler, GoalsService goalsService)
        {
            _goalsService = goalsService;
            _gameBoard = board;
            _itemsChain = chain;
            _boardFiller = filler;
            _itemCrusher = crusher;

            _inputService.Press += Press;
            _inputService.PressUp += PressUp;

            _goalsService.OnFinish += Restart;
            _itemCrusher.SendData += _goalsService.Receive;
            
            _config = JsonConvert.DeserializeObject<LevelConfig>(File.ReadAllText(path));

            StartGame();
        }

        private void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void StartGame()
        {
            _goalsService.Init(_config);
            _gameBoard.InitCells(_config);
            _boardFiller.InitFill();
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
            _inputService.Disable();
            await _itemCrusher.Crush();
            _itemCrusher.SendTurnData();

            _boardFiller.CreateModifier(_itemsChain.Count);
            _itemsChain.Clean();

            await _boardFiller.Fill();
            _inputService.Enable();

        }
    }
}