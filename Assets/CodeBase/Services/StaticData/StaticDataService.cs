// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CodeBase.BoardItems;
using CodeBase.BoardItems.Token;
using CodeBase.UIScripts;
using CodeBase.UIScripts.Data;
using Object = UnityEngine.Object;

namespace CodeBase.Services.StaticData
{
    public class StaticDataService
    {
        private const string ConfigsPath = "Prefabs/UI/UIConfig";
        private const string AtlasPath = "Tokens/TokenAtlas";
        private const string ItemsPath = "Sprites/ItemAtlas";

        private Dictionary<UIElement, Object> _uiConfings;
        private Dictionary<TokenType, Sprite> _tokenSprites;
        private Dictionary<ItemType, Sprite> _itemConfigs;

        public StaticDataService()
        {
            _uiConfings = Resources
                .Load<UIConfig>(ConfigsPath).Prefabs
                .ToDictionary(x => x.Type, x => x.Prefab);
            
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

        public Object ForUI(UIElement elementID) =>
            _uiConfings.TryGetValue(elementID, out Object prefab)
                ? prefab
                : null;
    }
}