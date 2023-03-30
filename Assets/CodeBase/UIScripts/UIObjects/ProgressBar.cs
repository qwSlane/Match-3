// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using UnityEngine;
using DG.Tweening;
using TMPro;

namespace CodeBase.UIScripts.UIObjects
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _current;
        [SerializeField] private TextMeshProUGUI _total;
        [SerializeField] private RectTransform _image;

        public TextMeshProUGUI Current => _current;

        private int _totalScore;
        private int _currentScore;

        public void Construct(int total)
        {
            _totalScore = total;
            _currentScore = 0;

            _total.text = total.ToString();
            _current.text = "0";

            _image.localScale = new Vector3(0, 1, 1);
        }

        public void UpdateScore(int current)
        {
            _currentScore += current;
            _current.text = _currentScore.ToString();

            var width = (float)_currentScore / _totalScore;
            if (width > 1)
            {
                width = 1;
            }
            _image.transform.DOScaleX(width, 0.3f);
        }

        public bool IsAchived() =>
            _currentScore >= _totalScore;
    }
}