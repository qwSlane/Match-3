// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.BoardItems.Token
{
    [CreateAssetMenu(fileName = "TokenAtlas", menuName = "Static data/TokenAtlas")]
    public class TokenAtlas : ScriptableObject
    {
        public List<TokenData> sprites = new List<TokenData>() { };
    }
}