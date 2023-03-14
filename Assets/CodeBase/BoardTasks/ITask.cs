// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System.Threading;
using Cysharp.Threading.Tasks;

namespace CodeBase.BoardTasks
{
    public interface ITask
    {
        public UniTask Execute(CancellationToken cancellationToken = default);
    }
}