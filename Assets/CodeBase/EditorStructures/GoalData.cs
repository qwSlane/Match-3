// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System;

namespace CodeBase.EditorStructures
{
    [Serializable]
    public class GoalData<T>
    {
        public T Type;
        public int Count;

        public GoalData(T type, int count)
        {
            Type = type;
            Count = count;
        }
    }
}