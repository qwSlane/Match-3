// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using CodeBase.Board.BoardKernel.Score;
using CodeBase.BoardItems;
using CodeBase.BoardItems.Cell;
using CodeBase.BoardItems.Modifiers;
using CodeBase.BoardItems.Obstacles;
using CodeBase.BoardTasks;
using CodeBase.Goals;

namespace CodeBase.Board.BoardKernel
{
    public class ItemCrusher
    {
        public Action<TurnData> SendData;

        private List<IGridCell> _additionalCrushes;

        private readonly GameBoard _board;
        private readonly ScoreSpawner _scoreSpawner;
        private readonly ItemsChain _chain;

        public ItemCrusher(GameBoard board, ScoreSpawner scoreSpawner, ItemsChain chain)
        {
            _board = board;
            _scoreSpawner = scoreSpawner;
            _chain = chain;
        }

        public async UniTask Crush()
        {
            _additionalCrushes = null;
            _additionalCrushes = AdditionalCrushes().ToList();

            IEnumerable<ITask> disappearTasks = _scoreSpawner.Spawn(_additionalCrushes);

            foreach (ITask task in disappearTasks)
            {
                await task.Execute();
            }
            Clean();
        }

        public void SendTurnData()
        {
            SendData?.Invoke(_scoreSpawner.TurnData);
        }

        private void Clean()
        {
            foreach (IGridCell cell in _additionalCrushes)
            {
                cell.Clear();
            }
            _additionalCrushes = null;
        }

        private IEnumerable<IGridCell> AdditionalCrushes()
        {
            IEnumerable<IGridCell> addcrushes = CrushObstacles(_chain.Chain);
            return addcrushes.Concat(CrushByModifier(_chain.Chain));
        }

        private IEnumerable<IGridCell> CrushObstacles(IEnumerable<IGridCell> chain)
        {
            List<IGridCell> obstacles = new List<IGridCell>();
            foreach (IGridCell cell in chain)
            {
                BoardPosition position = cell.Position;
                CrushObstacles(position, 1, obstacles);
                CrushObstacles(position, -1, obstacles);
            }
            return obstacles;
        }

        private IEnumerable<IGridCell> CrushByModifier(IEnumerable<IGridCell> chain)
        {
            foreach (IGridCell cell in chain)
            {
                if (cell.Item is IModifiable item)
                {
                    IEnumerable<BoardPosition> positions = item.UseModifiers();
                    if (positions != null)
                        foreach (BoardPosition position in positions)
                        {
                            if (_board.IsOnBoard(position.PosX, position.PosY) &&
                                _board[position].IsStorable)
                                yield return _board[position];
                        }
                }
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