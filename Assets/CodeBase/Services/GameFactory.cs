// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using UnityEngine;
using CodeBase.BoardItems;
using CodeBase.Services.AssetService;
using CodeBase.Board.BoardServices.Score;
using CodeBase.BoardItems.Cell;

namespace CodeBase.Services
{
    public class GameFactory
    {
        private readonly Dictionary<ItemType, string> _paths = new Dictionary<ItemType, string>()
        {
            [ItemType.Token] = "Prefabs/Token",
            [ItemType.Ice] = "Prefabs/Obstacles/Ice",
            [ItemType.Stone] = "Prefabs/Obstacles/Stone",
            [ItemType.ReinforcedStone] = "Prefabs/Obstacles/ReinforcedStone",
            [ItemType.Rocket] = "Prefabs/Modifiers/Rocket",
        };

        private IAssetProvider _iAssetProvider;

        private Stack<ScoreItem> _scoreItems;

        public GameFactory(IAssetProvider iAssetProvider)
        {
            _iAssetProvider = iAssetProvider;
            _scoreItems = new Stack<ScoreItem>();
        }

        public Cell Create(string path, Vector3 position, Transform parent)
        {
            Cell obj = Object.Instantiate(
                _iAssetProvider.Asset<Cell>(path), position, Quaternion.identity, parent
            );
            return obj;
        }

        public T Create<T>(ItemType itemType, Vector3 position, Transform parent) where T : Object
        {
            T obj = Object.Instantiate(
                _iAssetProvider.Asset<T>(_paths[itemType]), position, Quaternion.identity, parent
            );
            return obj;
        }

        public ScoreItem CreateScore(Transform parent)
        {
            ScoreItem item;
            if (_scoreItems.Count == 0)
            {
                item = Object.Instantiate(
                    _iAssetProvider.Asset<ScoreItem>("Prefabs/Score/Score"), Vector3.zero, Quaternion.identity, parent
                );

                item.Initialize(this);
                return item;
            }
            
            item = _scoreItems.Pop();
            item.transform.SetParent(parent);
            return item;
        }

        public void Reclaim(ScoreItem item)
        {
            item.transform.position = Vector3.zero;
            _scoreItems.Push(item);
        }
    }
}