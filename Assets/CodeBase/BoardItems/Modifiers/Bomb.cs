// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.BoardItems.Modifiers
{
    public class Bomb : MonoBehaviour, IModifier
    {
        [SerializeField] private int _length;
        private IModifier _modifierImplementation;

        public IEnumerable<BoardPosition> Use(Vector3 position)
        {
            var marked = new List<BoardPosition>();
            for (var i = -_length; i <= _length; i++)
            {
                for (var j = -_length; j <= _length; j++)
                {
                    marked.Add(new BoardPosition((int)position.x + i, (int)position.y + j));
                }
            }

            return marked;
        }

        public void SetParent(Transform parent)
        {
            transform.SetParent(parent);
        }

        public void Destroy()
        {
            gameObject.SetActive(false);
        }
    }
}