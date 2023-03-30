// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using UnityEngine;
using CodeBase.Board.BoardKernel.Score;
using CodeBase.BoardItems;
using CodeBase.Services.AssetService;
using CodeBase.BoardItems.Cell;
using CodeBase.BoardItems.Token;
using CodeBase.EditorStructures;

namespace CodeBase.Services
{
    public class GameFactory
    {
        private const string CellA = "Prefabs/GameBoard/CellA";
        private const string CellB = "Prefabs/GameBoard/CellB";
        private const string CellEmpty = "Prefabs/GameBoard/CellEmpty";

        private int _cellCount;

        private readonly Dictionary<ItemType, string> _paths = new Dictionary<ItemType, string>()
        {
            [ItemType.Token] = "Prefabs/Token",
            [ItemType.Ice] = "Prefabs/Obstacles/Ice",
            [ItemType.Stone] = "Prefabs/Obstacles/Stone",
            [ItemType.ReinforcedStone] = "Prefabs/Obstacles/ReinforcedStone",
            [ItemType.Rocket] = "Prefabs/Modifiers/Rocket",
            [ItemType.Bomb] = "Prefabs/Modifiers/Bomb",
        };

        private IAssetProvider _iAssetProvider;

        private Stack<ScoreItem> _scoreItems;
        private Queue<Token> _tokens;

        public GameFactory(IAssetProvider iAssetProvider)
        {
            _iAssetProvider = iAssetProvider;
            _scoreItems = new Stack<ScoreItem>();
            _tokens = new Queue<Token>();
            _cellCount = 0;
        }

        public Cell CreateCell(NodeType type, Vector3 position, Transform parent)
        {
            string path = (type == NodeType.Empty) ? CellEmpty :
                ((_cellCount & 1) == 0) ? CellA : CellB;

            var obj = Object.Instantiate(
                _iAssetProvider.Asset<Cell>(path), position, Quaternion.identity, parent
            );

            _cellCount++;
            return obj;
        }

        public T Create<T>(ItemType itemType, Vector3 position, Transform parent) where T : Object
        {
            var obj = Object.Instantiate(
                _iAssetProvider.Asset<T>(_paths[itemType]), position, Quaternion.identity, parent
            );
            return obj;
        }

        public Token CreateToken(Vector3 position, Transform parent)
        {
            Token token;
            if (_tokens.Count == 0)
            {
                token = Object.Instantiate(
                    _iAssetProvider.Asset<Token>(_paths[ItemType.Token]), position, Quaternion.identity, parent
                );
                return token;
            }

            token = _tokens.Dequeue();
            token.gameObject.SetActive(true);
            token.Transform.localScale = Vector3.one;
            token.Transform.position = position;

            return token;
        }

        public ScoreItem CreateScore(Transform parent)
        {
            ScoreItem item;
            if (_scoreItems.Count < 1)
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

        public void Reclaim(Token item)
        {
            item.Transform.position = Vector3.zero;
            item.gameObject.SetActive(false);
            if (!_tokens.Contains(item))
                _tokens.Enqueue(item);
        }
    }
}