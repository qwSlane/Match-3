// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using UnityEngine;

namespace CodeBase.BoardItems.Obstacles
{
    public class StoneObstacle : MonoBehaviour, IObstacle
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private int _durability;
        [SerializeField] private AudioSource _audio;
        [SerializeField] private AudioClip _crush;

        public bool IsMovable => true;
        public ItemType ItemType => ItemType.Stone;
        public SpriteRenderer SpriteRenderer => _spriteRenderer;
        public Transform Transform => transform;
        
        public void FallSound()
        {
            _audio.Play();
        }

        public void Reclaim()
        {
            _audio.clip = _crush;
            _audio.Play();
        }

        public bool Crash()
        {
            _durability -= 1;
            return _durability == 0;
        }
    }
}