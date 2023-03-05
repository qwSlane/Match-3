// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using CodeBase.Board.BoardServices.ItemMover;

namespace CodeBase.TaskRunner
{
    public class MoveTask: ITask
    {
        private const float MoveDuration = 0.3f;
        private const float PrependDuration = 0.09f;

        private readonly IEnumerable<MoveData> _moveData;

        public MoveTask(IEnumerable<MoveData> moveData)
        {
            _moveData = moveData;
        }

        public async UniTask Execute(CancellationToken cancellationToken = default)
        {
            Sequence sequence = DOTween.Sequence();
            foreach (MoveData moveData in _moveData)
            {
                _ = sequence
                    .Join(moveData.Item.Transform.DOPath(moveData.Path, MoveDuration * 1.5f))
                    .PrependInterval(PrependDuration);
            }
            await sequence.SetEase(Ease.Flash);
        }
    }
}