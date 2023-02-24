// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using CodeBase.Items;

namespace CodeBase.TaskRunner
{
    public class DissapearTask : ITask
    {
        private const float ScaleDuration = 0.3f;
        private const float FadeDuration = 0.3f;

        private readonly IEnumerable<IGriddable> _items;

        public DissapearTask(IEnumerable<IGriddable> items)
        {
            _items = items;
        }

        public async UniTask Execute(CancellationToken cancellationToken = default)
        {
            Sequence sequence = DOTween.Sequence();

            foreach (IGriddable griddable in _items)
            {
                _ = sequence
                    .Join(griddable.Item.transform.DOScale(Vector3.zero, ScaleDuration))
                    .Join(griddable.Item.SpriteRenderer.DOFade(0, FadeDuration));
            }

            await sequence.WithCancellation(cancellationToken);

            //reclaim items
        }

    }
}