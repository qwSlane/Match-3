// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

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

        public void Construct()
        {
            _closeButton.Button.onClick.AddListener(Close);
            _effects.Construct();
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
