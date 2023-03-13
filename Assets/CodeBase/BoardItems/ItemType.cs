// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CodeBase.BoardItems
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ItemType
    {
        Token,
        Ice,
        Stone,
        ReinforcedStone,
        Rocket,
        Bomb
    }
}