// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using DG.Tweening;
using CodeBase.Items;

namespace CodeBase.TaskRunner
{
    public class MoveTask
    {
        private const float MoveDuration = 0.3f;
        
        private GameBoard.GameBoard _gameBoard;

        public MoveTask(GameBoard.GameBoard gameBoard)
        {
            _gameBoard = gameBoard;
        }

        public void CreatePath()
        {
            Sequence sequence = DOTween.Sequence();
            for (int i = 0; i < _gameBoard.Columns; i++)
            {
                for (int j = 0; j < _gameBoard.Rows; j++)
                {
                    Cell cell = _gameBoard[i, j];
                    if (!cell.IsEmpty)
                    {
                        Item item = cell.Item;
                        cell.Clear();
                        BoardPosition destination = Create(i, j);
                        _gameBoard[destination.PosX, destination.PosY].Item = item;
                        sequence.Insert(j/10,
                            item.transform.DOMove(destination.ToVector(), MoveDuration));
                    }
                }
            }
            sequence.SetEase(Ease.Linear);
        }

        private BoardPosition Create(int column, int row)
        {
            bool isonBoard = _gameBoard.IsOnBoard(column, row - 1);
            bool isEmpty = false;
            if (isonBoard)
                isEmpty = _gameBoard[column, row - 1].IsEmpty;
            if (isonBoard && isEmpty)
            {
                return Create(column, row - 1);
            }
            return new BoardPosition(column, row);
        }
    }
}