// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using UnityEngine;

namespace CodeBase.Services.AssetService
{
    public interface IAssetProvider
    {
        T Asset<T>(string path) where T : Object;
    }
}