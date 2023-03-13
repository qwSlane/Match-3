// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CodeBase.Board.BoardServices.Mover;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace CodeBase.TaskRunner
{
    public class MoveTask : ITask
    {
        private const float MoveDuration = 0.45f;
        private const float PrependDuration = 0.004f;

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
                    .Join(moveData.Item.Transform.DOPath(moveData.Path, MoveDuration))
                    .InsertCallback(MoveDuration+PrependDuration, 
                        () => moveData.Item.FallSound())
                    .PrependInterval(PrependDuration);
            }
            await sequence.WithCancellation(default);
        }

      
    }
}