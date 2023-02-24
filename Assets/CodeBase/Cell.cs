// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System;
using UnityEngine;
using CodeBase.Items;

namespace CodeBase
{
    public class Cell : MonoBehaviour, IGriddable
    {
        public BoardPosition Position { get; private set; }

        public bool IsEmpty => _isEmpty;
        
        public bool IsMovable { get; }

        private Item _item;
        private bool _isEmpty;

        public Item Item
        {
            get => _item;
            set
            {
                _item = value;
                _isEmpty = false;
            }
        }

        public void Construct(int rowPos, int columnPos)
        {
            Position = new BoardPosition(rowPos, columnPos);
        }

        public void Clear()
        {
            Item = default;
            _isEmpty = true;
        }

       
    }
}