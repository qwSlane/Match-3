// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System;
using Object = UnityEngine.Object;

namespace CodeBase.UIScripts.Services.StaticData.Data
{
    [Serializable]
    public class UIPrefab
    {
        public UIElement Type;
        public Object Prefab;
    }
}