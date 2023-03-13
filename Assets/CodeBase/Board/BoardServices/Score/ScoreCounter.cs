// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using CodeBase.BoardItems;
using CodeBase.BoardItems.Cell;
using CodeBase.BoardItems.Token;
using CodeBase.Goals;
using CodeBase.Services;
using CodeBase.TaskRunner;

namespace CodeBase.Board.BoardServices.Score
{
    public class ScoreCounter
    {
        private const int Score = 20;

        public TurnData TurnData => new TurnData(_obstacles, _tokens, _turnScore);
        
        private readonly GameFactory _factory;
        private readonly ItemsChain _chain;

        private Dictionary<ItemType, int> _obstacles;
        private Dictionary<TokenType, int> _tokens;
        private int _turnScore;

        public ScoreCounter(GameFactory factory, ItemsChain chain)
        {
            _factory = factory;
            _chain = chain;

            _obstacles = new Dictionary<ItemType, int>();
            _tokens = new Dictionary<TokenType, int>();
        }

        public async UniTask Count(IEnumerable<IGridCell> additional)
        {
            ResetTurnData();

            DissapearTask task = new DissapearTask(ScoreForChain(), false);
            DissapearTask task1 = new DissapearTask(ScoreForAdditional(additional), true);

            await task.Execute();
            await task1.Execute();
            
        }

        private void ResetTurnData()
        {
            _obstacles.Clear();
            _tokens.Clear();
            _turnScore = 0;
        }

        private Dictionary<IGridCell, ScoreItem> ScoreForAdditional(IEnumerable<IGridCell> additional)
        {
            Dictionary<IGridCell, ScoreItem> toCrush = new Dictionary<IGridCell, ScoreItem>();
            foreach (IGridCell cell in additional)
            {
                ScoreItem item = _factory.CreateScore(cell.Transform);
                item.Text.SetText(Score.ToString());
                toCrush[cell] = item;

                WriteScore(cell);
                _turnScore += Score;
            }
            return toCrush;
        }

        private void WriteScore(IGridCell cell)
        {
            ItemType type = cell.Item.ItemType;

            if (type == ItemType.Token)
            {
                Token token = (Token)cell.Item;
                TokenType tokenType = token.TokenId;
                _tokens.TryGetValue(tokenType, out int i);
                _tokens[tokenType] = i + 1;
            }
            else
            {
                _obstacles.TryGetValue(type, out int count);
                _obstacles[type] = count + 1;
            }
        }

        private Dictionary<IGridCell, ScoreItem> ScoreForChain()
        {
            Dictionary<IGridCell, ScoreItem> toCrush = new Dictionary<IGridCell, ScoreItem>();
            int i = 0;

            foreach (IGridCell cell in _chain.Chain)
            {
                ScoreItem item = _factory.CreateScore(cell.Transform);
                int score = 50 + (i % 3) * 20;
                item.Text.SetText(score.ToString());
                toCrush[cell] = item;

                _turnScore += score;
                i++;
            }

            TokenType type = _chain.ChainType;
            _tokens[type] = toCrush.Count;

            return toCrush;
        }
    }
}