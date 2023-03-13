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
            List<BoardPosition> marked = new List<BoardPosition>();
            for (int i = -_length; i <= _length; i++)
            {
                for (int j = -_length; j <= _length; j++)
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