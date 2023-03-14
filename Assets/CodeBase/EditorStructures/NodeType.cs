// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CodeBase.EditorStructures
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum NodeType
    {
        Empty,
        Storable,
        Ice,
        Stone,
        ReinforcedStone
    }
}