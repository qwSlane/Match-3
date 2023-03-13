// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using UnityEngine;
using UnityEngine.UI;
using CodeBase.UIScripts.Services;
using CodeBase.UIScripts.Services.StaticData;
using CodeBase.UIScripts.UIObjects;
using TMPro;

namespace CodeBase.UIScripts
{
    public class UIKernel : MonoBehaviour
    {
        [SerializeField] private Transform _canvas;
        [SerializeField] private Transform _goalsParent;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private TextMeshProUGUI _turns;
        [SerializeField] private Transform _goalsContainer;

        public TextMeshProUGUI Turns => _turns;

        public Transform Canvas => _canvas;

        public Transform GoalsParent => _goalsParent;
        
        public Transform GoalsContainer => _goalsContainer;

        private SettingsPopup _settingsPopup = null;
        private UIFactory _factory;

        private void Awake()
        {
            _settingsButton.onClick.AddListener(OpenSettingsPopup);
            _factory = new UIFactory(new StaticDataService());
        }

        private void OpenSettingsPopup()
        {
            if (_settingsPopup == null)
                _settingsPopup = _factory.CreateSettingsPopup(_canvas);
            _settingsPopup.Construct();
            _settingsPopup.Open();
        }

        public void Restart()
        {
            
        }
    }
}