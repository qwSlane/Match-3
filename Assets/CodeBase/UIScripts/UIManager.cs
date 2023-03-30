// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using CodeBase.Services;
using CodeBase.UIScripts.Windows;

namespace CodeBase.UIScripts
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance = null;

        [SerializeField] private Button _settingsButton;
        [SerializeField] private InputService _inputService;

        private List<Window> _windows;
        private List<Window> _openedWindows;

        [Inject]
        public void Construct(List<Window> windows)
        {
            Instance = this;
            _windows = windows;
            _openedWindows = new List<Window>();
            InitUI();
        }

        private void InitUI()
        {
            _settingsButton.onClick.AddListener(Open<SettingsWindow>);
        }

        public void Open<T>() where T : Window
        {
            foreach (var window in _windows)
            {
                if (window is T)
                {
                    if (window.IsOpen)
                        return;
                    
                    _inputService.Disable();
                    _openedWindows.Add(window);
                    window.OnClose += Check;
                    window.Open();
                }
            }
        }

        private void Check(Window sender)
        {
            _openedWindows.Remove(sender);
            if (_openedWindows.Count == 0)
            {
                _inputService.Enable();
            }
        }
    }
}