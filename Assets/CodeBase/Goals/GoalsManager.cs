// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System.Linq;
using UnityEngine;

namespace CodeBase.Goals
{
    public class GoalsManager
    {
        public void Recieve(TurnData data)
        {
            Debug.Log("{" + string.Join(",", data.Obstacles.Select(kv => kv.Key + "=" + kv.Value).ToArray()) + "}");
            Debug.Log("{" + string.Join(",", data.Tokens.Select(kv => kv.Key + "=" + kv.Value).ToArray()) + "}");
            Debug.Log(data.Score);
        }
    }
}