// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using UnityEngine;

namespace CodeBase.BoardItems.Obstacles
{
    public class IceObstacle : MonoBehaviour, IObstacle
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private int _durability;

        public bool IsMovable => false;
        public ItemType ItemType => ItemType.Ice;
        public SpriteRenderer SpriteRenderer => _spriteRenderer;
        public Transform Transform => transform;

        public bool Crash()
        {
            _durability -= 1;
            return _durability == 0;
        }
    }
}