// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.BoardItems.Modifiers
{
    public interface IModifier
    {
        public IEnumerable<BoardPosition> Use(Vector3 position);
        public void SetParent(Transform parent);
        void Destroy();
    }
}