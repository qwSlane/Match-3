// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using CodeBase.BoardItems.Cell;
using CodeBase.BoardItems.Modifiers;
using CodeBase.Services;
using Object = UnityEngine.Object;

namespace CodeBase.BoardItems.Token
{
    public class Token : MonoBehaviour, ICellItem, IModifiable
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private AudioSource _audio;

        [SerializeField] private AudioClip _crush;
        [SerializeField] private AudioClip _fall;

        public bool IsMovable { get; private set; }

        public SpriteRenderer SpriteRenderer => _spriteRenderer;

        public Transform Transform => transform;

        public ItemType ItemType => ItemType.Token;

        public TokenType TokenId { get; private set; }

        public bool HasModifier => _modifier != null;

        private IModifier _modifier;
        private GameFactory _factory;

        public void Construct(GameFactory factory, Sprite sprite, TokenType token)
        {
            _factory = factory;
            _spriteRenderer.sprite = sprite;
            IsMovable = true;
            TokenId = token;
        }

        public void FallSound()
        {
            _audio.clip = _fall;
            _audio.Play();
        }

        public async void Reclaim()
        {
            _audio.clip = _crush;
            _audio.Play();
            CleanModifiers();
            await UniTask.Delay(TimeSpan.FromSeconds(_crush.length));
            Deselect();
            _factory.Reclaim(this);
        }

        private void CleanModifiers()
        {
            _modifier?.Destroy();
            _modifier = null;
        }

        private void Deselect()
        {
            SpriteRenderer.color = Color.white;
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