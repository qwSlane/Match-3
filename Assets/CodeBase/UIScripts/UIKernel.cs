// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using CodeBase.Services;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CodeBase.Services.StaticData;
using CodeBase.UIScripts.Services;
using CodeBase.UIScripts.UIObjects;
using Zenject;

namespace CodeBase.UIScripts
{
    public class UIKernel : MonoBehaviour
    {
        [SerializeField] private Transform _canvas;
        [SerializeField] private Transform _goalsParent;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private TextMeshProUGUI _turns;
        [SerializeField] private Transform _goalsContainer;

        [SerializeField] private InputService _inputService;

        public TextMeshProUGUI Turns => _turns;

        public Transform Canvas => _canvas;

        public Transform GoalsParent => _goalsParent;

        public Transform GoalsContainer => _goalsContainer;

        private SettingsPopup _settingsPopup = null;
        private UIFactory _factory;

        [Inject]
        private void Initialize(UIFactory factory)
        {
            _settingsButton.onClick.AddListener(OpenSettingsPopup);
            _factory = factory;
        }

        private void OpenSettingsPopup()
        {
            if (_settingsPopup == null)
            {
                _settingsPopup = _factory.CreateSettingsPopup(_canvas);
                _settingsPopup.Construct(_inputService.Enable);
            }
            _settingsPopup.Open();
            _inputService.Disable();
        }

        public void DisableInput()
        {
            _inputService.Disable();
        }
    }
}