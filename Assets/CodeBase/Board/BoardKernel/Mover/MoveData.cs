// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using UnityEngine;
using CodeBase.BoardItems.Cell;

namespace CodeBase.Board.BoardKernel.Mover
{
    public class MoveData
    {
        public ICellItem Item { get; }
        public Vector3[] Path { get; }

        public MoveData(ICellItem item, Vector3[] path)
        {
            Item = item;
            Path = path;
        }
    }
}