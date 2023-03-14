// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using CodeBase.Board.BoardKernel.Mover;

namespace CodeBase.BoardTasks
{
    public class AppearTask
    {
        private const float MoveDuration = 0.45f;
        private const float FadeDuration = 0.3f;
        private const float PrependDuration = 0.004f;

        private readonly IEnumerable<MoveData> _moveData;

        public AppearTask(IEnumerable<MoveData> moveData)
        {
            _moveData = moveData;
        }

        public async UniTask Execute(CancellationToken cancellationToken = default)
        {
            Sequence sequence = DOTween.Sequence();
            foreach (MoveData moveData in _moveData)
            {
                _ = sequence
                    .Join(moveData.Item.Transform.DOPath(moveData.Path, MoveDuration))
                    .Join(moveData.Item.SpriteRenderer.DOFade(1, FadeDuration))
                    .InsertCallback(MoveDuration+PrependDuration, 
                        () => moveData.Item.FallSound())
                    .PrependInterval(PrependDuration);
            }

            await sequence.WithCancellation(cancellationToken);
        }
    }
}