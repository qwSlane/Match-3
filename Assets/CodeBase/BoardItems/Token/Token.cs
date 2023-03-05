// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using UnityEngine;
using CodeBase.BoardItems.Cell;
using CodeBase.BoardItems.Modifiers;

namespace CodeBase.BoardItems.Token
{
    public class Token : MonoBehaviour, ICellItem, IModifiable
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        public bool IsMovable { get; private set; }

        public SpriteRenderer SpriteRenderer => _spriteRenderer;
        public Transform Transform => transform;

        public ItemType ItemType => ItemType.Token;

        public TokenType TokenId { get; private set; }

        public bool HasModifier => _modifier != null;

        private IModifier _modifier;

        public void Construct(Sprite sprite, TokenType token)
        {
            _spriteRenderer.sprite = sprite;
            IsMovable = true;
            TokenId = token;
        }

        public IEnumerable<BoardPosition> UseModifiers()
        {
            return _modifier?.Use(transform.position);
        }

        public void AddModifier(IModifier modifier)
        {
            _modifier = modifier;
            _modifier.SetParent(transform);
        }
    }
}