// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System;
using CodeBase.BoardItems;

namespace CodeBase.Structures
{
    [Serializable]
    public class CellData
    {
        public BoardPosition Position;
        public NodeType Type;

        public CellData(BoardPosition position, NodeType type)
        {
            Position = position;
            Type = type;
        }
    }
}