// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using UnityEngine;

namespace CodeBase.Editor.Structures
{
    [CreateAssetMenu(fileName = "LevelTools", menuName = "Level creator/Level Tools", order = 0)]
    public class LevelTools : ScriptableObject
    {
        public ToolAsset[] FieldTools;
    }
}