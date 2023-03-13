// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SwapSlider : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    [SerializeField] private SpriteSwapper _background;
    [SerializeField] private SpriteSwapper _button;

    [SerializeField] private Animation _animation;
    [SerializeField] private AudioSource _audio;

    [SerializeField] private AudioMixer _mixer;

    [SerializeField] private string _parameter;

    public void Construct()
    {
        _slider.onValueChanged.AddListener(Slide);
    }

    private void Slide(float value)
    {
        _animation.Play();
        _mixer.SetFloat(_parameter, (value != 0) ? value : -80f);
        _audio.Play();

        _background.Swap(value);
        _button.Swap(value);
    }
}