// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    [SerializeField] private Button _restart;

    public void Construct(Action action)
    {
        _restart.onClick.AddListener(action.Invoke);
    }
}