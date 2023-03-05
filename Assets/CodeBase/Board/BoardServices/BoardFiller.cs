// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using CodeBase.BoardItems;
using CodeBase.BoardItems.Cell;
using CodeBase.BoardItems.Modifiers;
using CodeBase.Board.BoardServices.ItemMover;
using CodeBase.BoardItems.Obstacles;
using CodeBase.BoardItems.Token;
using CodeBase.Services;
using CodeBase.Services.AssetService;
using CodeBase.TaskRunner;

namespace CodeBase.Board.BoardServices
{
    public class BoardFiller
    {
        private const float MoveDuration = 0.3f;
        private const float FadeDuration = 0.15f;

        private readonly GameBoard _board;
        private readonly ItemMover.ItemMover _mover;
        private readonly GameFactory _gameFactory;
        private readonly Transform _parent;
        private readonly IAssetProvider _assetProvider;

        private readonly List<IGridCell> _spawnerCells;
        private Dictionary<ItemType, int> _modifiers;

        public BoardFiller(
            GameFactory gameFactory, GameBoard board,
            Transform parent, IAssetProvider assetProvider
        )
        {
            _gameFactory = gameFactory;
            _board = board;
            _mover = new ItemMover.ItemMover(board);
            _parent = parent;
            _assetProvider = assetProvider;
            _spawnerCells = new List<IGridCell>();
            _modifiers = new Dictionary<ItemType, int>();

            InitSpawnerCells();
        }

        public void Init()
        {
            var sequence = DOTween.Sequence();
            for (int i = 0; i < _board.Columns; i++)
            {
                for (int j = 0; j < _board.Rows; j++)
                {
                    Cell cell = _board[i, j];
                    ItemType itemType = (ItemType)Random.Range(0, 2);
                    Vector3 position = new Vector3(cell.transform.position.x, cell.transform.position.y, 0);

                    switch (itemType)
                    {
                        case ItemType.Token:
                            cell.Item = ConstructToken(position);
                            break;
                        case ItemType.Ice:
                            cell.Item = _gameFactory.Create<IceObstacle>(itemType, position, _parent);
                            break;
                        /* case ItemType.Stone:
                             cell.Item = _gameFactory.Create<StoneObstacle>(itemType, position, _parent);
                             break;
                        case ItemType.ReinforcedStone:
                            cell.Item = _gameFactory.Create<ReinforcedStoneObstacle>(itemType, position, _parent);
                            break;*/
                    }
                    sequence.Join(cell.Item.SpriteRenderer.DOFade(1, FadeDuration));
                }
            }
            sequence.SetEase(Ease.Linear);
            _mover.InitializeItemsPaths();
        }

        public async UniTask Fill()
        {
            await _mover.SlideDown();
            await _mover.FallDown();

            List<MoveData> moveData = new List<MoveData>();
            foreach (IGridCell cell in _spawnerCells)
            {
                while (cell.IsEmpty)
                {
                    List<Vector3> path = new List<Vector3>();
                    Vector3 initPosition = new Vector3(cell.Position.PosX, _board.Rows, 0);

                    Token token = ConstructToken(initPosition);
                    path.Add(new Vector3(cell.Position.PosX, cell.Position.PosY, 0));

                    _mover.CreatePath(cell.Position.PosX, cell.Position.PosY, path);
                    _board[(int)path[path.Count - 1].x, (int)path[path.Count - 1].y].Item = token;

                    moveData.Add(new MoveData(token, path.ToArray()));
                }
            }
            moveData.Reverse();
            AppearTask task = new AppearTask(moveData);

            await task.Execute();

            SetupModifiers();
        }

        private void SetupModifiers()
        {
            foreach (KeyValuePair<ItemType, int> modifier in _modifiers)
            {
                for (int i = 0; i < modifier.Value; i++)
                {
                    BoardPosition position = _board.ModifiableItem();
                    Token item = (Token)_board[position].Item;

                    item.AddModifier(_gameFactory.Create<Rocket>(modifier.Key, item.Transform.position,
                        item.Transform));
                }
            }
            _modifiers.Clear();
        }

        public void CreateModifier(int size)
        {
            if (size is >= 5 and < 8)
            {
                _modifiers.TryGetValue(ItemType.Rocket, out int val);
                _modifiers[ItemType.Rocket] = val + 1;
            }
        }

        private Token ConstructToken(Vector3 position)
        {
            Token token = _gameFactory.Create<Token>(ItemType.Token, position, _parent);
            TokenType tokenType = (TokenType)Random.Range(0, 5);
            Sprite sprite = _assetProvider.TokenSprite(tokenType);
            token.Construct(sprite, tokenType);
            return token;
        }

        private void InitSpawnerCells()
        {
            for (int i = 0; i < _board.Columns; i++)
            {
                for (int j = 0; j < _board.Rows; j++)
                {
                    if (j == _board.Rows - 1)
                    {
                        _spawnerCells.Add(_board[i, j]);
                    }

                    if (HaveNoNeighbours(i, j) == false)
                    {
                        _spawnerCells.Add(_board[i, j]);
                    }
                }
            }
        }

        private bool HaveNoNeighbours(int col, int row)
        {
            return _board.IsNeighbour(col, row + 1) ||
                   _board.IsNeighbour(col + 1, row - 1) ||
                   _board.IsNeighbour(col - 1, row - 1);
        }
    }
}