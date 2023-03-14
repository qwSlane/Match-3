// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;

namespace CodeBase.BoardItems.Modifiers
{
    public interface IModifiable
    {
        public bool HasModifier { get; }
        public void AddModifier(IModifier modifier);
        public IEnumerable<BoardPosition> UseModifiers();
    }
}