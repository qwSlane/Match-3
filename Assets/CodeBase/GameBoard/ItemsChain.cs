// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using CodeBase.Items;
using CodeBase.TaskRunner;

namespace CodeBase.GameBoard
{
    public class ItemsChain
    {
        private Stack<IGriddable> _chain;
        private ItemType _chainType;

        public ItemsChain()
        {
            _chain = new Stack<IGriddable>();
        }

        public void AddElement(IGriddable selected)
        {
            if (_chain.Count == 0)
            {
                _chainType = selected.Item.ItemType;
                _chain.Push(selected);
                selected.Item.Select();
                return;
            }

            if (_chain.Contains(selected))
            {
                while (_chain.Count > 0)
                {
                    IGriddable element = _chain.Pop();
                    if (element.Item != selected.Item)
                    {
                        element.Item.Deselect();
                    }
                    else
                    {
                        _chain.Push(element);
                        break;
                    }
                }
            }
            else
            {
                if (selected.Position.Near(_chain.Peek().Position) && selected.Item.ItemType == _chainType)
                {
                    _chain.Push(selected);
                    selected.Item.Select();
                }
            }
        }

        public async UniTask Apply()
        {
            if (_chain.Count >= 3)
            {
                ITask dissapear = new DissapearTask(_chain);

                await dissapear.Execute();
            }
            foreach (IGriddable cell in _chain)
            {
                cell.Clear();
            }
            _chain.Clear();
         
        }
    }
}