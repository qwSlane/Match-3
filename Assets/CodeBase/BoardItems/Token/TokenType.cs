// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CodeBase.BoardItems.Token
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TokenType 
    {
        Red,
        Blue,
        Yellow,
        Pink,
        Green
    }
}