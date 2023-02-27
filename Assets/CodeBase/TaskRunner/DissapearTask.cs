// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using CodeBase.BoardItems.Cell;

namespace CodeBase.TaskRunner
{
    public class DissapearTask : ITask
    {
        private const float ScaleDuration = 0.3f;
        private const float FadeDuration = 0.3f;

        private readonly IEnumerable<IGridCell> _items;

        public DissapearTask(IEnumerable<IGridCell> items)
        {
            _items = items;
        }

        public async UniTask Execute(CancellationToken cancellationToken = default)
        {
            Sequence sequence = DOTween.Sequence();

            foreach (IGridCell cell in _items)
            {
                _ = sequence
                    .Join(cell.Item.Transform.DOScale(Vector3.zero, ScaleDuration))
                    .Join(cell.Item.SpriteRenderer.DOFade(0, FadeDuration));
            }

            await sequence.WithCancellation(cancellationToken);

            //reclaim items
        }
    }
}