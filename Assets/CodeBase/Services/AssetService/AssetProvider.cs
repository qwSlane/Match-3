// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CodeBase.BoardItems.Token;

namespace CodeBase.Services.AssetService
{
    public class AssetProvider : IAssetProvider
    {
        private const string AtlasPath = "Tokens/TokenAtlas";

        private Dictionary<string, Object> _assets = new Dictionary<string, Object>();
        private Dictionary<TokenType, Sprite> _tokenSprites;

        public AssetProvider()
        {
            _tokenSprites = Resources.Load<TokenAtlas>(AtlasPath).sprites
                .ToDictionary(x => x.Type, x => x.Sprite);
        }

        public Sprite TokenSprite(TokenType type) => _tokenSprites[type];

        public T Asset<T>(string path) where T : Object
        {
            T asset;
            if (_assets.TryGetValue(path, out Object value))
            {
                asset = (T)_assets[path];
                return asset;
            }

            asset = Resources.Load<T>(path);
            _assets[path] = asset;

            return asset;
        }
    }
}