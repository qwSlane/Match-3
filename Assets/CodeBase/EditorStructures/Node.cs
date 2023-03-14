// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System;
using UnityEngine;

namespace CodeBase.EditorStructures
{
    public class Node
    {
        [NonSerialized] public GUIStyle Style;

        public NodeType Type;
        private Rect _rect;

        public Node(Vector2 position, float width, float height, GUIStyle style)
        {
            _rect = new Rect(position.x, position.y, width, height);
            Type = NodeType.Storable;
            Style = style;
        }

        public void Draw()
        {
            GUI.Box(_rect, "", Style);
        }

        public void SetStyle(GUIStyle style, NodeType type)
        {
            Style = style;
            Type = type;
        }

        public void Drag(Vector2 delta)
        {
            _rect.position += delta;
        }
    }
}