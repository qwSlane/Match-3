// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using UnityEngine;

namespace CodeBase.BoardItems.Cell
{
    public interface IGridCell
    {
        public BoardPosition Position { get; }

        public Transform Transform { get; }

        public bool IsEmpty { get; }

        public bool IsStorable { get; }

        public ICellItem Item { get; }

        public void Clear();
    }
}