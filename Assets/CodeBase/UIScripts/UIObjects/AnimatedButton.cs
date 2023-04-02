// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UIScripts.UIObjects
{
    public class AnimatedButton : MonoBehaviour
    {
        [SerializeField] protected Button _button;
        [SerializeField] private Animation _animation;
        [SerializeField] private AudioSource _audio;

        private void Awake()
        {
            _button.onClick.AddListener(Animate);
        }

        private void Animate()
        {
            _animation.Play();
            _audio.Play();
        }
    }
}
