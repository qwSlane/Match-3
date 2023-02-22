using System.Collections.Generic;
using CodeBase.Items;
using UnityEditor;
using UnityEngine;

namespace CodeBase.Grid
{
    public class ItemsChain
    {
        private Stack<Item> _chain;
        private ItemType _chainType;

        public ItemsChain()
        {
            _chain = new Stack<Item>();
        }

        public void AddElement(Item selected)
        {
            if (_chain.Count == 0)
            {
                _chainType = selected.ItemType;
                _chain.Push(selected);
                selected.Select();
                return;
            }

            if (_chain.Contains(selected))
            {
                while (_chain.Count > 0)
                {
                    Item element = _chain.Pop();
                    if (element != selected)
                    {
                        element.Deselect();
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
                if (selected.WorldPosition.Near(_chain.Peek().WorldPosition) && selected.ItemType == _chainType)
                {
                    _chain.Push(selected);
                    selected.Select();
                }
            }
        }

        public void Apply()
        {
            foreach (Item item in _chain)
            {
                item.SpriteRenderer.color = Color.white;
            }

            _chain.Clear();
        }
    }
}