// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using CodeBase.Board.BoardKernel.Mover;
using CodeBase.BoardItems;
using CodeBase.BoardItems.Cell;
using CodeBase.BoardItems.Modifiers;
using CodeBase.BoardItems.Token;
using CodeBase.BoardTasks;
using CodeBase.Services;
using CodeBase.Services.AssetService;

namespace CodeBase.Board.BoardKernel
{
    public class BoardFiller
    {
        private const float FadeDuration = 0.15f;

        private readonly GameBoard _board;
        private readonly Mover.ItemMover _mover;
        private readonly GameFactory _gameFactory;
        private readonly IAssetProvider _assetProvider;

        private Dictionary<ItemType, int> _modifiers;
        private readonly List<IGridCell> _spawnerCells;

        public BoardFiller(GameFactory gameFactory, GameBoard board)
        {
            _gameFactory = gameFactory;
            _board = board;
            _mover = new Mover.ItemMover(board);

            _spawnerCells = new List<IGridCell>();
            _modifiers = new Dictionary<ItemType, int>();
        }

        public void InitFill()
        {
            InitSpawnerCells();
            
            var sequence = DOTween.Sequence();
            foreach (var boardCell in _board.Cells)
            {
                if (boardCell.IsStorable)
                    sequence.Join(boardCell.Item.SpriteRenderer.DOFade(1, FadeDuration));
            }

            sequence.SetEase(Ease.Linear);
            _mover.InitializeItemsPaths();
        }

        public async UniTask Fill()
        {
            await _mover.SlideDown();
            await _mover.FallDown();

            List<MoveData> moveData = new List<MoveData>();
            foreach (var cell in _spawnerCells)
            {
                while (cell.IsEmpty)
                {
                    List<Vector3> path = new List<Vector3>();
                    Vector3 initPosition = new Vector3(cell.Position.PosX, _board.Rows, 0);

                    Token token = _board.ConstructToken(initPosition);
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
            foreach (var modifier in _modifiers)
            {
                for (var i = 0; i < modifier.Value; i++)
                {
                    var position = _board.ModifiableItem();
                    var item = (Token)_board[position].Item;

                    var location = new Vector3(item.Transform.position.x, item.Transform.position.y, -1);

                    switch (modifier.Key)
                    {
                        case ItemType.Bomb:
                            item.AddModifier(_gameFactory.Create<Bomb>(modifier.Key, location,
                                item.Transform));
                            break;
                        case ItemType.Rocket:
                            item.AddModifier(_gameFactory.Create<Rocket>(modifier.Key, location,
                                item.Transform));
                            break;
                    }
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
            if (size >= 8)
            {
                _modifiers.TryGetValue(ItemType.Bomb, out int val);
                _modifiers[ItemType.Bomb] = val + 1;
            }
        }

        private void InitSpawnerCells()
        {
            for (var i = 0; i < _board.Columns; i++)
            {
                for (var j = 0; j < _board.Rows; j++)
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
                   _board.IsNeighbour(col + 1, row + 1) ||
                   _board.IsNeighbour(col - 1, row + 1);
        }
    }
}