// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

namespace CodeBase
{
    public static class Extensions
    {
        public static string ValueOrEmpty(int v)
        {
            return (string.IsNullOrEmpty(v.ToString()) || v == 0)
                ? ""
                : v.ToString();
        }
    }
}