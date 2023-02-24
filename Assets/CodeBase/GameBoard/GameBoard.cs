// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using DG.Tweening;
using UnityEngine;
using CodeBase.Services;
using CodeBase.Items;

namespace CodeBase.GameBoard
{
    public class GameBoard
    {
        private const float MoveDuration = 0.3f;
        private const float FadeDuration = 0.25f;
        
        public int Rows => _rows;

        public int Columns => _columns;

        private int _rows;
        private int _columns;
        private Cell[,] _gridSlots;

        public void InitBoard(Cell[,] gridSlots)
        {
            _columns = gridSlots.GetLength(0);
            _rows = gridSlots.GetLength(1);
            _gridSlots = gridSlots;
        }

        public Cell this[int column, int row] => _gridSlots[column, row];

        public bool IsOnBoard(BoardPosition position) =>
            (position.PosX >= 0 && position.PosX < _columns) && (position.PosY >= 0 && position.PosY < _rows);

        public bool IsOnBoard(int column, int row) =>
            (column >= 0 && column < _columns) && (row >= 0 && row < _rows);

        public void Fill(GameFactory gameFactory, Transform parent)
        {
            var sequence = DOTween.Sequence();
            for (int i = 0; i < _columns; i++)
            {
                var seq = DOTween.Sequence();
                for (int j = 0; j < _rows; j++)
                {
                    Cell cell = _gridSlots[i, j];
                    ItemType type = (ItemType)Random.Range(0, 5);
                    Vector3 position = new Vector3(i, _rows, -1);
                    cell.Item = gameFactory.Create(type, position, parent);
                    _gridSlots[i, j] = cell;

                    Vector3 destination = new Vector3(cell.transform.position.x, cell.transform.position.y, 0);
                    seq.Join(cell.Item.transform.DOMove(destination, MoveDuration));
                    sequence.Join(cell.Item.SpriteRenderer.DOFade(1, FadeDuration));
                }
                sequence.Insert(i * 0.05f, seq);
            }

            sequence.SetEase(Ease.Linear);
        }
    }
}