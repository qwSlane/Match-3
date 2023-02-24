// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using UnityEngine;
using CodeBase.Items.Modifiers;

namespace CodeBase.Items
{
    public class Item : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        public bool IsMovable { get; private set; }

        public BoardPosition WorldPosition
        {
            get =>new ((int)transform.position.x, (int)transform.position.y);
            set
            {
                transform.position = new Vector3(value.PosX, value.PosY, 0);
            }
        }
        
        public SpriteRenderer SpriteRenderer => _spriteRenderer;

        public ItemType ItemType { get; private set; }

        private List<IModifier> _modifiers = new List<IModifier>();

        public void Construct(ItemType itemType, Sprite sprite)
        {
            ItemType = itemType;
            _spriteRenderer.sprite = sprite;
            IsMovable = true;
        }

        public void AddModifier(IModifier modifier)
        {
            _modifiers.Add(modifier);
        }

        public void Deselect()
        {
            SpriteRenderer.color = Color.white;
        }

        public void Select()
        {
            SpriteRenderer.color = Color.red;
        }
    }
}