// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.BoardItems.Token
{
    [CreateAssetMenu(fileName = "ItemAtlas", menuName = "Static data/ItemAtlas")]
    public class ItemsAtlas : ScriptableObject
    {
        public List<ItemData> sprites = new List<ItemData>() { };
    }
}