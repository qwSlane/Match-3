// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using UnityEngine;
using CodeBase.BoardItems;
using CodeBase.Services.AssetService;
using CodeBase.BoardItems.Cell;
using CodeBase.BoardItems.Token;

namespace CodeBase.Services
{
    public class GameFactory
    {
        private IAssetProvider _iAssetProvider;

        public GameFactory(IAssetProvider iAssetProvider)
        {
            _iAssetProvider = iAssetProvider;
        }

        public Cell Create(string path, Vector3 position, Transform parent)
        {
            Cell obj = Object.Instantiate(
                _iAssetProvider.Asset<Cell>(path), position, Quaternion.identity, parent
            );
            return obj;
        }

        public Token Create(ItemType itemType, Vector3 position, Transform parent)
        {
            Token obj = Object.Instantiate(
                _iAssetProvider.Asset<Token>("Item"), position, Quaternion.identity, parent
            );
            TokenType tokenType = (TokenType)Random.Range(0, 5);
            Sprite sprite = _iAssetProvider.TokenSprite(tokenType);
            obj.Construct(itemType, sprite, tokenType);
            return obj;
        }

        public T CreateObstacle<T>(ItemType itemType, Vector3 position, Transform parent) where T : Object, ICellItem
        {
            T obj = Object.Instantiate(
                _iAssetProvider.Asset<T>("Ice"), position, Quaternion.identity, parent
            );
            return obj;
        }
    }
}