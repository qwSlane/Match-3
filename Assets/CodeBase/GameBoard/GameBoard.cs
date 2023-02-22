// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using CodeBase.Items;
using CodeBase.Services;

namespace CodeBase.Grid
{
    public class GameBoard
    {
        public int Rows => _rows;

        public int Columns => _columns;

        private Stack<Renderer> _highlited = new Stack<Renderer>();
        private int _rows;
        private int _columns;
        private Cell[,] _gridSlots;
        
        public void InitBoard(Cell[,] gridSlots)
        {
            _rows = gridSlots.GetLength(0);
            _columns = gridSlots.GetLength(1);
            _gridSlots = gridSlots;
        }
        
        public bool IsOnBoard(BoardPosition position) =>
            (position.PosX >= 0 && position.PosX < _rows) && (position.PosY >= 0 && position.PosY < _columns);

        public Cell Cell(int row, int column) => _gridSlots[row, column];

        public void Fill(GameFactory gameFactory)
        {
            var sequence = DOTween.Sequence();
            for (int j = 0; j < _columns; j++)
            {
                var seq = DOTween.Sequence();
                for (int i = 0; i < _rows; i++)
                {
                    Cell cell = _gridSlots[i, j];
                    ItemType type = (ItemType)Random.Range(0, 5);
                    Vector3 position = new Vector3(i, _columns, -1);
                    cell.Item = gameFactory.Create(type, position, cell.transform);

                    _gridSlots[i, j] = cell;

                    Vector3 destination = new Vector3(cell.transform.position.x, cell.transform.position.y, 0);
                    seq.Join(cell.Item.transform.DOMove(destination, 0.3f));
                    sequence.Join(cell.Item.SpriteRenderer.DOFade(1, 0.25f));
                }
                sequence.Insert(j * 0.05f, seq);
            }

            sequence.SetEase(Ease.Linear);
        }
    }
}