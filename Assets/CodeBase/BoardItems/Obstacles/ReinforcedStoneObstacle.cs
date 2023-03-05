// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using UnityEngine;

namespace CodeBase.BoardItems.Obstacles
{
    public class ReinforcedStoneObstacle : MonoBehaviour, IObstacle
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Sprite _damaged;
        [SerializeField] private int _durability;

        public bool IsMovable => true;
        public ItemType ItemType => ItemType.Ice;
        public SpriteRenderer SpriteRenderer => _spriteRenderer;
        public Transform Transform => transform;

        public bool Crash()
        {
            _durability -= 1;
            if (_durability != 0)
            {
                _spriteRenderer.sprite = _damaged;
            }
            return _durability == 0;
        }
    }
}