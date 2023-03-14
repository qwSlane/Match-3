// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CodeBase.UIScripts.UIObjects
{
    public class UIGoal : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _current;
        [SerializeField] private TextMeshProUGUI _total;
        [SerializeField] private Image _image;

        public Image Image => _image;

        private int _totalCount;
        private int _currentCount;

        public void Construct(int total)
        {
            _totalCount = total;
            _currentCount = 0;

            _total.text = _totalCount.ToString();
            _current.text = "0";
        }

        public bool CountUpdate(int value)
        {
            _currentCount += value;
            if (_currentCount >= _totalCount)
            {
                _currentCount = _totalCount;
                _current.text = _currentCount.ToString();
                return true;
            }
            _current.text = _currentCount.ToString();
            return false;
        }
    }
}