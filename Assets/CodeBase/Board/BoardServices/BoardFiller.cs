// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using CodeBase.BoardItems;
using CodeBase.BoardItems.Cell;
using CodeBase.BoardItems.Obstacles;
using CodeBase.BoardItems.Token;
using CodeBase.Services;
using CodeBase.TaskRunner;

namespace CodeBase.Board.BoardServices
{
    public class BoardFiller
    {
        private const float MoveDuration = 0.3f;
        private const float FadeDuration = 0.15f;

        private GameBoard _board;
        private List<IGridCell> _spawnerCells;
        private MoveTask _mover;
        private readonly GameFactory _gameFactory;
        private Transform _parent;

        public BoardFiller(GameFactory gameFactory, GameBoard board, MoveTask mover, Transform parent)
        {
            _gameFactory = gameFactory;
            _board = board;
            _mover = mover;
            _parent = parent;
            _spawnerCells = new List<IGridCell>();
            InitSpawnerCells();
        }

        public async UniTask Fill()
        {
            Sequence sequence = DOTween.Sequence();

            foreach (IGridCell cell in _spawnerCells)
            {
                while (cell.IsEmpty)
                {
                    List<Vector3> path = new List<Vector3>();
                    Vector3 initPosition = new Vector3(cell.Position.PosX, _board.Rows, 0);

                    Token token = _gameFactory.Create(ItemType.Token, initPosition, _parent);
                    path.Add(new Vector3(cell.Position.PosX, cell.Position.PosY, 0));

                    _mover.CreatePath(cell.Position.PosX, cell.Position.PosY, path);
                    _board[(int)path[path.Count - 1].x, (int)path[path.Count - 1].y].Item = token;
                    _ = sequence
                        .Join(token.Transform.DOPath(path.ToArray(), MoveDuration))
                        .Join(token.SpriteRenderer.DOFade(1, FadeDuration));
                }
            }

            await sequence.SetEase(Ease.Flash);
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
                            cell.Item = _gameFactory.Create(itemType, position, _parent);
                            break;
                        case ItemType.Ice:
                            cell.Item = _gameFactory.CreateObstacle<IceObstacle>(itemType, position, _parent);
                            break;
                    }
                    sequence.Join(cell.Item.SpriteRenderer.DOFade(1, FadeDuration));
                }
            }
            sequence.SetEase(Ease.Linear);
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