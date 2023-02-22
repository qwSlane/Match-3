// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using UnityEngine;
using CodeBase.Items;

namespace CodeBase
{
    public class Cell : MonoBehaviour
    {
        public BoardPosition _Position;
        public bool IsEmpty { get; private set; }

        public bool IsStorable { get; set; }

        public Item Item { get; set; }

        public void Construct(int rowPos, int columnPos)
        {
            _Position = new BoardPosition(rowPos, columnPos);
        }

        public void Clear()
        {
            IsEmpty = false;
        }
    }
}