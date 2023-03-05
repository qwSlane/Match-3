// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using CodeBase.BoardItems;
using CodeBase.BoardItems.Token;

namespace CodeBase.Goals
{
    public class TurnData
    {
        public Dictionary<ItemType, int> Obstacles { get; }
        
        public Dictionary<TokenType, int> Tokens { get; }

        public int Score { get; }

        public TurnData(Dictionary<ItemType, int> obstacles, Dictionary<TokenType, int> tokens, int score)
        {
            Obstacles = obstacles;
            Tokens = tokens;
            Score = score;
        }
    }
}