// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using CodeBase.Board.BoardServices.Score;
using CodeBase.BoardItems.Cell;
using CodeBase.Services;

namespace CodeBase.TaskRunner
{
    public class DissapearTask : ITask
    {
        private const float ScaleDuration = 0.3f;
        private const float FadeDuration = 0.3f;

        private float PrependDuration;
        private readonly Dictionary<IGridCell, ScoreItem> _items;
        private GameFactory _factory;
        

        public DissapearTask(Dictionary<IGridCell, ScoreItem> items, bool isParralel)
        {
            _items = items;
            PrependDuration = (isParralel) ? 0 : ScaleDuration * FadeDuration;
        }

        public async UniTask Execute(CancellationToken cancellationToken = default)
        {
            Sequence sequence = DOTween.Sequence();
            foreach (KeyValuePair<IGridCell, ScoreItem> pair in _items)
            {
                _ = sequence
                    .Join(pair.Key.Item.Transform.DOScale(Vector3.zero, ScaleDuration))
                    .Join(pair.Key.Item.SpriteRenderer.DOFade(0, FadeDuration))
                    .InsertCallback(PrependDuration, async () => await pair.Value.Play())
                    .PrependInterval(PrependDuration);
            }

            await sequence.WithCancellation(cancellationToken);
        }
    }
}