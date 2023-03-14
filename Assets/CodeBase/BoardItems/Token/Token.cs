// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using CodeBase.BoardItems.Cell;
using CodeBase.BoardItems.Modifiers;
using CodeBase.Services;

namespace CodeBase.BoardItems.Token
{
    public class Token : MonoBehaviour, ICellItem, IModifiable
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private GameObject _selected;
        [SerializeField] private AudioSource _audio;

        [SerializeField] private AudioClip _crush;
        [SerializeField] private AudioClip _select;
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
            _selected.SetActive(false);
            _factory.Reclaim(this);
        }

        private void CleanModifiers()
        {
            _modifier?.Destroy();
            _modifier = null;
        }

        public void Deselect(float pitch)
        {
            _selected.SetActive(false);
            _audio.clip = _select;
            _audio.pitch = pitch / 2;
            _audio.Play();
        }

        public void Select(float pitch)
        {
            _selected.SetActive(true);
            _audio.clip = _select;
            _audio.pitch = pitch / 2;
            _audio.Play();
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