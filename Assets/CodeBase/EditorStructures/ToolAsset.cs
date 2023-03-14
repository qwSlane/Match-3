// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System;
using UnityEngine;

namespace CodeBase.EditorStructures
{
    [Serializable]
    public struct ToolAsset
    {
        public NodeType Type;
        public Texture2D Icon;
        public string Hint;

        [HideInInspector]
        public GUIStyle Style;

        public static bool operator ==(ToolAsset s1, ToolAsset s2) => 
            (s1.Icon == s2.Icon) && (s1.Hint == s2.Hint) && (s1.Type == s2.Type);

        public static bool operator !=(ToolAsset s1, ToolAsset s2) => 
            !(s1 == s2);

        public bool IsEmpty() => 
            (Icon == null);

        public bool Equals(ToolAsset other)
        {
            return Type == other.Type && Equals(Icon, other.Icon) && Hint == other.Hint && Equals(Style, other.Style);
        }

        public override bool Equals(object obj)
        {
            return obj is ToolAsset other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine((int)Type, Icon, Hint, Style);
        }
    }
}