// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System;
using UnityEngine;

namespace CodeBase.BoardItems
{
    public class BoardPosition
    {
        public int PosX { get; }

        public int PosY { get; }

        public BoardPosition(int posX, int posY)
        {
            PosX = posX;
            PosY = posY;
        }

        public BoardPosition(Vector2 rayOrigin)
        {
            PosX = (int)Math.Round(rayOrigin.x);
            PosY = (int)Math.Round(rayOrigin.y);
        }

        public bool Near(BoardPosition position)
        {
            int max = Math.Max(Math.Abs(PosX - position.PosX), Math.Abs(PosY - position.PosY));
            return max <= 1;
        }

        public Vector3 ToVector() => new(PosX, PosY, 0);

        public static BoardPosition operator -(BoardPosition pos1, BoardPosition pos2)
        {
            return new BoardPosition(pos1.PosX - pos2.PosX, pos1.PosY - pos2.PosY);
        }
    }
}