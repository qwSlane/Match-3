using System.Reflection;
using CodeBase.UIScripts;
using CodeBase.UIScripts.UIObjects;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Tests
{
    public class UITests
    {
        [TestFixture]
        public class UIGoalTests
        {
            private UIGoal _uiGoal;
            private TextMeshProUGUI _currentText;
            private TextMeshProUGUI _totalText;
            private Image _image;

            [SetUp]
            public void SetUp()
            {
                GameObject uiGoalObject = new GameObject("UIGoal");
                _uiGoal = uiGoalObject.AddComponent<UIGoal>();

                _currentText = new GameObject("CurrentText").AddComponent<TextMeshProUGUI>();
                _currentText.transform.SetParent(uiGoalObject.transform);
                _uiGoal.GetType().GetField("_current", BindingFlags.NonPublic | BindingFlags.Instance)
                    .SetValue(_uiGoal, _currentText);

                _totalText = new GameObject("TotalText").AddComponent<TextMeshProUGUI>();
                _totalText.transform.SetParent(uiGoalObject.transform);
                _uiGoal.GetType().GetField("_total", BindingFlags.NonPublic | BindingFlags.Instance)
                    .SetValue(_uiGoal, _totalText);

                _image = new GameObject("Image").AddComponent<Image>();
                _image.transform.SetParent(uiGoalObject.transform);
                _uiGoal.GetType().GetField("_image", BindingFlags.NonPublic | BindingFlags.Instance)
                    .SetValue(_uiGoal, _image);
            }

            [Test]
            public void Construct_SetsTotalCount_AndInitializesTexts()
            {
                // Arrange
                int totalCount = 5;

                // Act
                _uiGoal.Construct(totalCount);

                // Assert
                Assert.AreEqual(totalCount.ToString(), _totalText.text);
                Assert.AreEqual("0", _currentText.text);
            }

            [Test]
            public void CountUpdate_IncrementsCurrentCount_AndUpdatesTexts()
            {
                // Arrange
                int totalCount = 5;
                _uiGoal.Construct(totalCount);

                // Act
                bool isGoalReached = _uiGoal.CountUpdate(2);

                // Assert
                Assert.AreEqual("2", _currentText.text);
                Assert.IsFalse(isGoalReached);
            }

            [Test]
            public void CountUpdate_ReachesGoal_AndUpdatesTexts()
            {
                // Arrange
                int totalCount = 5;
                _uiGoal.Construct(totalCount);

                // Act
                bool isGoalReached = _uiGoal.CountUpdate(totalCount);

                // Assert
                Assert.AreEqual(totalCount.ToString(), _currentText.text);
                Assert.IsTrue(isGoalReached);
            }
        }
    }

    [TestFixture]
    public class ProgressBarTests
    {
        private ProgressBar _progressBar;
        private TextMeshProUGUI _currentText;
        private TextMeshProUGUI _totalText;
        private RectTransform _image;

        [SetUp]
        public void SetUp()
        {
            GameObject progressBarObject = new GameObject("ProgressBar");
            _progressBar = progressBarObject.AddComponent<ProgressBar>();

            _currentText = new GameObject("CurrentText").AddComponent<TextMeshProUGUI>();
            _currentText.transform.SetParent(progressBarObject.transform);
            _progressBar.GetType().GetField("_current", BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(_progressBar, _currentText);

            _totalText = new GameObject("TotalText").AddComponent<TextMeshProUGUI>();
            _totalText.transform.SetParent(progressBarObject.transform);
            _progressBar.GetType().GetField("_total", BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(_progressBar, _totalText);

            _image = new GameObject("Image").AddComponent<RectTransform>();
            _image.transform.SetParent(progressBarObject.transform);
            _progressBar.GetType().GetField("_image", BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(_progressBar, _image);
        }

        [Test]
        public void IsAchieved_ReturnsTrue_WhenCurrentScoreReachesTotalScore()
        {
            // Arrange
            int totalScore = 10;
            _progressBar.Construct(totalScore);
            _progressBar.UpdateScore(totalScore);

            // Act
            bool isAchieved = _progressBar;

            // Assert
            Assert.IsTrue(isAchieved);
        }
    }

  
}