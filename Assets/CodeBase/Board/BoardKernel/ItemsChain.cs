﻿// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using CodeBase.BoardItems;
using CodeBase.BoardItems.Cell;
using CodeBase.BoardItems.Modifiers;
using CodeBase.BoardItems.Token;

namespace CodeBase.Board.BoardKernel
{
    public class ItemsChain
    {
        public int Count => _chain.Count;

        public IEnumerable<IGridCell> Chain => _chain;

        public TokenType ChainType => _chainType;

        private Stack<IGridCell> _chain;
        private TokenType _chainType;

        public ItemsChain()
        {
            _chain = new Stack<IGridCell>();
        }

        public void AddElement(IGridCell selected)
        {
            if (_chain.Contains(selected))
            {
                PopElement(selected);
                return;
            }
            Push(selected);
        }

        public void Deselect()
        {
            foreach (var cell in _chain)
            {
                (cell.Item as Token).Deselect(_chain.Count);
            }
            _chain.Clear();
        }

        public void Clean()
        {
            foreach (var cell in _chain)
            {
                cell.Clear();
            }
            _chain.Clear();
        }

        private void Push(IGridCell selected)
        {
            if (_chain.Count == 0)
            {
                if (selected.Item.ItemType == ItemType.Token)
                {
                    var item = selected.Item as Token;
                    _chainType = item.TokenId;
                }
                _chain.Push(selected);
                (selected.Item as Token).Select(_chain.Count);
                return;
            }

            if (selected.Position.Near(_chain.Peek().Position) && ((Token)selected.Item).TokenId == _chainType)
            {
                _chain.Push(selected);
                (selected.Item as Token)?.Select(_chain.Count);
            }
        }

        private void PopElement(IGridCell selected)
        {
            while (_chain.Count > 0)
            {
                IGridCell element = _chain.Pop();
                if (element.Item != selected.Item)
                {
                    (element.Item as Token).Deselect(_chain.Count);
                }
                else
                {
                    _chain.Push(element);
                    break;
                }
            }
        }

        public bool IsModifier()
        {
            if (_chain.Count == 1)
            {
                if (_chain.Peek().Item is IModifiable modifiable)
                {
                    return modifiable.HasModifier;
                }
            }
            return false;
        }
    }
}