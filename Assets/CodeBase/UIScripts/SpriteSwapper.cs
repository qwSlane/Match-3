// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using UnityEngine;
using UnityEngine.UI;

public class SpriteSwapper : MonoBehaviour
{
    [SerializeField] private Image _source;

    [SerializeField] private Sprite _enable;
    [SerializeField] private Sprite _disable;

    public void Swap(float value)
    {
        _source.sprite = (value != 0) ? _enable : _disable;
    }
}