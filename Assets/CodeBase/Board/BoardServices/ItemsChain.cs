// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using CodeBase.BoardItems;
using CodeBase.BoardItems.Cell;
using CodeBase.BoardItems.Obstacles;
using CodeBase.BoardItems.Token;
using CodeBase.TaskRunner;

namespace CodeBase.Board.BoardServices
{
    public class ItemsChain
    {
        private Stack<IGridCell> _chain;
        private GameBoard _board;
        private TokenType _chainType;

        public ItemsChain(GameBoard board)
        {
            _board = board;
            _chain = new Stack<IGridCell>();
        }

        public void AddElement(IGridCell selected)
        {
            if (_chain.Count == 0)
            {
                if (selected.Item.ItemType == ItemType.Token)
                {
                    Token item = selected.Item as Token;
                    _chainType = item.TokenId;
                }
                _chain.Push(selected);
                selected.Item.SpriteRenderer.color = Color.red;
                return;
            }

            if (_chain.Contains(selected))
            {
                while (_chain.Count > 0)
                {
                    IGridCell element = _chain.Pop();
                    if (element.Item != selected.Item)
                    {
                        element.Item.SpriteRenderer.color = Color.white;
                    }
                    else
                    {
                        _chain.Push(element);
                        break;
                    }
                }
            }
            else
            {
                if (selected.Position.Near(_chain.Peek().Position) && ((Token)selected.Item).TokenId == _chainType)
                {
                    _chain.Push(selected);
                    selected.Item.SpriteRenderer.color = Color.red;
                }
            }
        }

        public async UniTask Apply()
        {
            if (_chain.Count >= 3)
            {
                CrushObstacles();
                ITask dissapear = new DissapearTask(_chain);
                await dissapear.Execute();

                foreach (IGridCell cell in _chain)
                {
                    cell.Clear();
                }
                _chain.Clear();
            }
        }

        private void CrushObstacles()
        {
            List<IGridCell> _obstacles = new List<IGridCell>();
            foreach (IGridCell cell in _chain)
            {
                BoardPosition position = cell.Position;
                CrushObstacles(position, 1, _obstacles);
                CrushObstacles(position, -1, _obstacles);
            }

            foreach (IGridCell cell in _obstacles)
            {
                _chain.Push(cell);
            }
        }

        private void CrushObstacles(BoardPosition position, int direction, List<IGridCell> gridCells)
        {
            if (_board.IsNeighbour(position.PosX + direction, position.PosY) &&
                !_board[position.PosX + direction, position.PosY].IsEmpty
               )
            {
                if (_board[position.PosX + direction, position.PosY].Item is IObstacle)
                {
                    IObstacle obs = (IObstacle)_board[position.PosX + direction, position.PosY].Item;
                    if (obs.Crash())
                    {
                        gridCells.Add(_board[position.PosX + direction, position.PosY]);
                    }
                }
            }

            if (_board.IsNeighbour(position.PosX, position.PosY + direction) &&
                !_board[position.PosX, position.PosY + direction].IsEmpty
               )
            {
                if (_board[position.PosX, position.PosY + direction].Item is IObstacle)
                {
                    IObstacle obs = (IObstacle)_board[position.PosX, position.PosY + direction].Item;
                    if (obs.Crash())
                    {
                        gridCells.Add(_board[position.PosX, position.PosY + direction]);
                    }
                }
            }
        }
    }
}