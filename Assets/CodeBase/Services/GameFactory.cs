// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using UnityEngine;
using CodeBase.Items;
using CodeBase.Services.AssetService;

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
                _iAssetProvider.GetAsset<Cell>(path), position, Quaternion.identity, parent
            );
            return obj;
        }

        public Item Create(ItemType itemType, Vector3 position, Transform parent)
        {
            Item obj = Object.Instantiate(
                _iAssetProvider.GetAsset<Item>("Item"), position, Quaternion.identity, parent
            );
            Sprite sprite = _iAssetProvider.GetAsset<Sprite>($"Items/{itemType}");
            obj.Construct(itemType, sprite);
            return obj;
        }
    }
}