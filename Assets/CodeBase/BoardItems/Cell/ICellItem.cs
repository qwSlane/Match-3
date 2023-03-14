// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using UnityEngine;

namespace CodeBase.BoardItems.Cell
{
    public interface ICellItem
    {
        public bool IsMovable { get; }

        public ItemType ItemType { get; }

        public SpriteRenderer SpriteRenderer { get; }

        public Transform Transform { get; }

        public void FallSound();
        public void Reclaim();
    }
}