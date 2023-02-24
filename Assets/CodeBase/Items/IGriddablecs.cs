// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

namespace CodeBase.Items
{
    public interface IGriddable
    {
        public BoardPosition Position { get; }

        public bool IsMovable { get; }

        public bool IsEmpty { get; }

        public Item Item { get; set; }

        public void Clear();
    }
}