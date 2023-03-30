// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using CodeBase.UIScripts.UIObjects;
using UnityEngine;

namespace CodeBase.UIScripts.Windows
{
    public class SettingsWindow : Window
    {
        private const string OpenAnimation = "Open";
        private const string CloseAnimation = "Close";

        [SerializeField] private Animation _animation;
        [SerializeField] private CloseButton _closeButton;

        [SerializeField] private SwapSlider _soundEffects;
        [SerializeField] private SwapSlider _music;

        private void Initialize()
        {
            _closeButton.Button.onClick.AddListener(Close);
            _soundEffects.Construct();
            _music.Construct();
        }

        protected override void SelfOpen()
        {
            Initialize();
            _animation.Play(OpenAnimation);
        }

        protected override void SelfClose()
        {
            _closeButton.Close();
            _animation.Play(CloseAnimation);
        }
    }
}