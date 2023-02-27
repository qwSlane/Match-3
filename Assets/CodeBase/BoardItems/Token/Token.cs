// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using UnityEngine;
using CodeBase.BoardItems.Cell;

namespace CodeBase.BoardItems.Token
{
    public class Token : MonoBehaviour, ICellItem
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        public bool IsMovable { get; private set; }

        public SpriteRenderer SpriteRenderer => _spriteRenderer;
        public Transform Transform => transform;

        public ItemType ItemType { get; private set; }

        public TokenType TokenId { get; private set; }

        public void Construct(ItemType itemType, Sprite sprite, TokenType token)
        {
            ItemType = itemType;
            _spriteRenderer.sprite = sprite;
            IsMovable = true;
            TokenId = token;
        }
    }
}