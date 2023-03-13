// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System;
using UnityEngine;

namespace CodeBase.BoardItems
{
    [Serializable]
    public class BoardPosition
    {
        public int PosX { get; set; }

        public int PosY { get; set; }

        public BoardPosition(int posX, int posY)
        {
            PosX = posX;
            PosY = posY;
        }

        public BoardPosition()
        {
        }

        public BoardPosition(BoardPosition position)
        {
            PosX = position.PosX;
            PosY = position.PosY;
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
        public Vector3 ToVectorBehind() => new(PosX, PosY, 1);

        public static BoardPosition operator -(BoardPosition pos1, BoardPosition pos2)
        {
            return new BoardPosition(pos1.PosX - pos2.PosX, pos1.PosY - pos2.PosY);
        }
    }
}