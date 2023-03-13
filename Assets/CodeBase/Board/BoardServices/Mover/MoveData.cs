// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using CodeBase.BoardItems.Cell;
using UnityEngine;

namespace CodeBase.Board.BoardServices.Mover
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