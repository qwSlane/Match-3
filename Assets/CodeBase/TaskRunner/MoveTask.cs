// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using CodeBase.Board;
using CodeBase.BoardItems;
using CodeBase.BoardItems.Cell;

namespace CodeBase.TaskRunner
{
    public class MoveTask
    {
        private const float MoveDuration = 0.3f;

        private GameBoard _gameBoard;
        private readonly Dictionary<IGridCell, List<Vector3>> _itemPath;

        public MoveTask(GameBoard gameBoard)
        {
            _gameBoard = gameBoard;
            _itemPath = new Dictionary<IGridCell, List<Vector3>>();
        }

        public void InitializeItemsPaths()
        {
            foreach (Cell cell in _gameBoard.Cells)
            {
                _itemPath[cell] = new List<Vector3>();
            }
        }

        public async UniTask FallDown()
        {
            Sequence sequence = DOTween.Sequence();
            for (int j = 0; j < _gameBoard.Rows; j++)
            {
                for (int i = 0; i < _gameBoard.Columns; i++)
                {
                    Cell cell = _gameBoard[i, j];
                    if (cell.IsEmpty == false && cell.Item.IsMovable)
                    {
                        ICellItem item = cell.Item;
                        cell.Clear();
                        List<Vector3> path = _itemPath[cell];
                        path.Clear();

                        CreatePath(i, j, path);
                        if (path.Count >= 1)
                        {
                            _ = sequence.Join(
                                item.Transform.DOPath(path.ToArray(), MoveDuration));
                            _gameBoard[(int)path[path.Count - 1].x, (int)path[path.Count - 1].y].Item = item;
                        }
                        else
                        {
                            cell.Item = item;
                        }
                    }
                }
            }
            await sequence.SetEase(Ease.Flash);
        }

        public void CreatePath(int col, int row, List<Vector3> path)
        {
            if (_gameBoard.CanMoveDown(col, row))
            {
                BoardPosition destination = MoveDown(col, row);
                path.Add(destination.ToVector());
                CreatePath(destination.PosX, destination.PosY, path);
                return;
            }
            if (_gameBoard.DiagonalRight(col, row))
            {
                path.Add(new Vector3(col + 1, row - 1));
                CreatePath(col + 1, row - 1, path);
                return;
            }
            if (_gameBoard.DiagonalLeft(col, row))
            {
                path.Add(new Vector3(col - 1, row - 1));
                CreatePath(col - 1, row - 1, path);
                return;
            }
        }

        private BoardPosition MoveDown(int column, int row)
        {
            if (_gameBoard.CanMoveDown(column, row))
            {
                return MoveDown(column, row - 1);
            }
            return new BoardPosition(column, row);
        }
    }
}