// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using CodeBase.BoardItems.Cell;

namespace CodeBase.BoardItems.Obstacles
{
    public interface IObstacle : ICellItem
    {
        public bool Crash();
    }
}