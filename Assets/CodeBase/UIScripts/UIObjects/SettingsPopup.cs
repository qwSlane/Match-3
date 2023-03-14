// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System;
using UnityEngine;

namespace CodeBase.UIScripts.UIObjects
{
    public class SettingsPopup : MonoBehaviour
    {
        private const string OpenAnimation = "Open";
        private const string CloseAnimation = "Close";
    
        [SerializeField] private Animation _animation;
        [SerializeField] private CloseButton _closeButton;

        [SerializeField] private SwapSlider _effects;
        [SerializeField] private SwapSlider _music;

        public void Construct(Action action)
        {
            _closeButton.Button.onClick.AddListener(Close);
            _closeButton.Button.onClick.AddListener(action.Invoke);
            _effects.Construct();
            _music.Construct();
        }

        public void Open()
        {
            _animation.Play(OpenAnimation);
        }
    
        public void Close()
        {
            _closeButton.Close();
            _animation.Play(CloseAnimation);
        }
    }
}
