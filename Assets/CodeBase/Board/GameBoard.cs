// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using UnityEngine;
using CodeBase.BoardItems;
using CodeBase.BoardItems.Cell;
using CodeBase.BoardItems.Modifiers;

namespace CodeBase.Board
{
    public class GameBoard
    {
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

        public IGridCell this[BoardPosition position] => _gridSlots[position.PosX, position.PosY];

        public Cell this[int column, int row] => _gridSlots[column, row];

        public Cell[,] Cells => _gridSlots;

        public bool IsOnBoard(BoardPosition position) =>
            (position.PosX >= 0 && position.PosX < _columns) && (position.PosY >= 0 && position.PosY < _rows);

        public bool IsOnBoard(int column, int row) =>
            (column >= 0 && column < _columns) && (row >= 0 && row < _rows);

        public bool IsNeighbour(int col, int row) =>
            (IsOnBoard(col, row) && _gridSlots[col, row].IsStorable);

        public bool DiagonalRight(int col, int row)
        {
            if (IsOnBoard(col + 1, row - 1) && _gridSlots[col + 1, row - 1].IsEmpty)
            {
                if (!IsNeighbour(col + 1, row) || CloseTop(col + 1, row))
                    return true;
            }
            return false;
        }

        private bool CloseTop(int col, int row)
        {
            if (IsOnBoard(col, row) && (_gridSlots[col, row].IsEmpty == false
                                        && _gridSlots[col, row].Item.IsMovable == false))
            {
                return true;
            }

            if (row == _rows - 1)
            {
                return false;
            }
            return !IsNeighbour(col, row + 1) || CloseTop(col, row + 1);
        }

        public bool DiagonalLeft(int col, int row)
        {
            return IsOnBoard(col - 1, row - 1) && _gridSlots[col - 1, row - 1].IsEmpty &&
                   (!IsNeighbour(col - 1, row) || CloseTop(col - 1, row));
        }

        public bool CanMoveDown(int col, int row) =>
            IsOnBoard(col, row - 1) && _gridSlots[col, row - 1].IsEmpty;

        public bool Unmovable(int col, int row) =>
            IsOnBoard(col, row) &&
            (_gridSlots[col, row].IsEmpty == false && _gridSlots[col, row].Item.IsMovable == false);

        public BoardPosition ModifiableItem()
        {
            int x = Random.Range(0, _columns);
            int y = Random.Range(0, _rows);

            //
            while (_gridSlots[x, y].IsEmpty && !(_gridSlots[x, y].Item is IModifiable) &&
                   !(_gridSlots[x, y].Item as IModifiable).HasModifier)
            {
                x = Random.Range(0, _columns);
                y = Random.Range(0, _rows);
            }
            return new BoardPosition(x, y);
        }
    }
}