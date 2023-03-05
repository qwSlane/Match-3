// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using UnityEngine;

namespace CodeBase.BoardItems.Cell
{
    public class Cell : MonoBehaviour, IGridCell
    {
        public BoardPosition Position { get; private set; }
        
        public Transform Transform => transform;

        public bool IsEmpty => _isEmpty;

        public bool IsStorable { get; private set; }

        private ICellItem _item;
        private bool _isEmpty;

        public ICellItem Item
        {
            get => _item;
            set
            {
                _item = value;
                _isEmpty = false;
            }
        }

        public void Construct(int rowPos, int columnPos, bool isStorable)
        {
            Position = new BoardPosition(rowPos, columnPos);
            IsStorable = isStorable;
        }

        public void Clear()
        {
            Item = default;
            _isEmpty = true;
        }
    }
}