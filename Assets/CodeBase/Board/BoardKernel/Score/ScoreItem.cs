// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System;
using UnityEngine;
using CodeBase.Services;
using TMPro;
using Cysharp.Threading.Tasks;

namespace CodeBase.Board.BoardKernel.Score
{
    public class ScoreItem : MonoBehaviour
    {
        [SerializeField] private TextMeshPro _scoreText;
        [SerializeField] private Animation _animation;

        public TextMeshPro Text => _scoreText;

        private GameFactory _gameFactory;

        public void Initialize(GameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }

        public async UniTask Play()
        {
            _scoreText.faceColor = UnityEngine.Random.ColorHSV(
                hueMin: 0f, hueMax: 1f,
                saturationMin: 0.6f, saturationMax: 1f,
                valueMin: 0.25f, valueMax: 1f,
                alphaMin: 1f, alphaMax: 1f
            );

            gameObject.SetActive(true);
            _animation.Play();
            await UniTask.Delay(TimeSpan.FromSeconds(.5f));
            gameObject.SetActive(false);

            _gameFactory.Reclaim(this);
        }
    }
}