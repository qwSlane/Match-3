// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.UIScripts.Data
{
    [CreateAssetMenu(fileName = "UIConfig", menuName = "Static data/UI static data", order = 0)]
    public class UIConfig : ScriptableObject
    {
        public List<UIPrefab> Prefabs;
    }
}