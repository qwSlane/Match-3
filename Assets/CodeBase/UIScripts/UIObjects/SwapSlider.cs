// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace CodeBase.UIScripts.UIObjects
{
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
            _mixer.GetFloat(_parameter, out float v);

            float value = (v == 0) ? 1 : 0;

            _slider.value = value;
            _background.Swap(value);
            _button.Swap(value);
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
}