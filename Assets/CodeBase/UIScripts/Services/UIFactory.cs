// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using UnityEngine;
using Unity.VisualScripting;
using CodeBase.Services.StaticData;
using CodeBase.UIScripts.UIObjects;

namespace CodeBase.UIScripts.Services
{
    public class UIFactory
    {
        private StaticDataService _dataService;

        public UIFactory(StaticDataService dataService)
        {
            _dataService = dataService;
        }

        public SettingsPopup CreateSettingsPopup(Transform canvas)
        {
            return Object.Instantiate(_dataService.ForUI(UIElement.SettingsPopup), canvas)
                .GetComponent<SettingsPopup>();
        }

        public UIGoal CreateGoal(Transform parent)
        {
            return Object.Instantiate(_dataService.ForUI(UIElement.GoalPrefab), parent)
                .GetComponent<UIGoal>();
        }

        public ProgressBar CreateProgressBar(Transform parent)
        {
            return Object.Instantiate(_dataService.ForUI(UIElement.ProgressBar), parent)
                .GetComponent<ProgressBar>();
        }

        public Popup CreateWinPopup(Transform canvas)
        {
            return Object.Instantiate(_dataService.ForUI(UIElement.WinPopup), canvas)
                .GetComponent<Popup>();
        }

        public Popup CreateLosePopUp(Transform canvas)
        {
            return Object.Instantiate(_dataService.ForUI(UIElement.LosePopup), canvas)
                .GetComponent<Popup>();
        }
    }
}