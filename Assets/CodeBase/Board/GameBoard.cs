// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using UnityEngine;
using CodeBase.BoardItems;
using CodeBase.BoardItems.Cell;
using CodeBase.BoardItems.Modifiers;
using CodeBase.BoardItems.Obstacles;
using CodeBase.BoardItems.Token;
using CodeBase.EditorStructures;
using CodeBase.Services;
using CodeBase.Services.StaticData;

namespace CodeBase.Board
{
    public class GameBoard
    {
        public int Rows => _rows;

        public int Columns => _columns;

        private int _rows;
        private int _columns;
        private Cell[,] _gridSlots;

        private readonly Transform _parent;
        private readonly GameFactory _factory;
        private readonly StaticDataService _dataService;

        public GameBoard(Transform parent, GameFactory factory, StaticDataService dataService)
        {
            _parent = parent;
            _factory = factory;
            _dataService = dataService;
        }

        public void InitCells(LevelConfig config)
        {
            _columns = config.FieldColumn;
            _rows = config.FieldRows;
            _gridSlots = new Cell[_columns, _rows];

            foreach (var cellData in config.Field)
            {
                var cell = _factory.CreateCell(cellData.Type, cellData.Position.ToVectorBehind(), _parent);
                cell.Construct(cellData.Position);
                _gridSlots[cellData.Position.PosX, cellData.Position.PosY] = cell;
            }

            InitCellItems(config);
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

        public Token ConstructToken(Vector3 position)
        {
            var tokenType = (TokenType)Random.Range(0, 5);
            var token = _factory.CreateToken(position, _parent);
            var sprite = _dataService.ForToken(tokenType);
            token.Construct(_factory, sprite, tokenType);
            return token;
        }

        public BoardPosition ModifiableItem()
        {
            var x = Random.Range(0, _columns);
            var y = Random.Range(0, _rows);

            while (!_gridSlots[x, y].IsStorable || _gridSlots[x, y].IsEmpty ||
                   _gridSlots[x, y].Item.ItemType != ItemType.Token ||
                   ((IModifiable)_gridSlots[x, y].Item).HasModifier)
            {
                x = Random.Range(0, _columns);
                y = Random.Range(0, _rows);
            }

            return new BoardPosition(x, y);
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

        private void InitCellItems(LevelConfig levelConfig)
        {
            foreach (var data in levelConfig.Field)
            {
                var position = data.Position.ToVector();
                ICellItem item = null;

                switch (data.Type)
                {
                    case NodeType.Storable:
                        item = ConstructToken(position);
                        break;
                    case NodeType.Ice:
                        item = _factory.Create<IceObstacle>(ItemType.Ice, position, _parent);
                        break;
                    case NodeType.Stone:
                        item = _factory.Create<StoneObstacle>(ItemType.Stone, position, _parent);
                        break;
                    case NodeType.ReinforcedStone:
                        item = _factory.Create<ReinforcedStoneObstacle>(ItemType.ReinforcedStone, position, _parent);
                        break;
                }
                _gridSlots[data.Position.PosX, data.Position.PosY].Item = item;
            }
        }
    }
}