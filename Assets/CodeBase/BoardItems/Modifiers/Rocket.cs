// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.BoardItems.Modifiers
{
    public class Rocket : MonoBehaviour, IModifier
    {
        [SerializeField] private int _length;
        private IModifier _modifierImplementation;

        public IEnumerable<BoardPosition> Use(Vector3 position)
        {
            List<BoardPosition> marked = new List<BoardPosition>();
            for (int i = 0; i < _length; i++)
            {
                marked.Add(new BoardPosition(i, (int)position.y));
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