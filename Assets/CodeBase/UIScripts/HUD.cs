// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using UnityEngine;
using TMPro;
using CodeBase.UIScripts.UIObjects;


namespace CodeBase.UIScripts
{
    public class HUD : MonoBehaviour
    {
        [SerializeField] private Transform _goalsContainer;
        [SerializeField] private TextMeshProUGUI _turns;
        [SerializeField] private Transform _scoreContainer;
        
        [SerializeField] private UIGoal _goalPrefab;
        [SerializeField] private ProgressBar _progressBarPrefab;

        public TextMeshProUGUI Turns => _turns;

        public ProgressBar InitProgressbar(int configScore)
        {
            var bar = Instantiate(_progressBarPrefab, _scoreContainer);
            bar.Construct(configScore);
            return bar;
        }

        public UIGoal CreateGoal(int count, Sprite image)
        {
            var goal = Instantiate(_goalPrefab, _goalsContainer);
            goal.Image.sprite = image;
            goal.Construct(count);
            return goal;
        }
    }
}