// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CodeBase.BoardItems;
using CodeBase.BoardItems.Token;

namespace CodeBase.Services.StaticData
{
    public class StaticDataService
    {
        private const string AtlasPath = "Tokens/TokenAtlas";
        private const string ItemsPath = "Sprites/ItemAtlas";
        
        private Dictionary<TokenType, Sprite> _tokenSprites;
        private Dictionary<ItemType, Sprite> _itemConfigs;

        public StaticDataService()
        {
  
            _tokenSprites = Resources.Load<TokenAtlas>(AtlasPath).sprites
                .ToDictionary(x => x.Type, x => x.Sprite);

            _itemConfigs = Resources
                .Load<ItemsAtlas>(ItemsPath).sprites
                .ToDictionary(x => x.Type, x => x.Sprite);
        }
        
        public Sprite ForToken(TokenType type) =>
            _tokenSprites.TryGetValue(type, out Sprite sprite)
                ? sprite
                : null;

        public Sprite ForItem(ItemType type) =>
            _itemConfigs.TryGetValue(type, out Sprite sprite)
                ? sprite
                : null;
        
    }
}