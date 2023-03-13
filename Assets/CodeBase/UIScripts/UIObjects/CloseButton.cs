// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UIScripts.UIObjects
{
    public class CloseButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Animation _animation;
        [SerializeField] private AudioSource _audio;

        public Button Button => _button;
    
        public void Close()
        {
            _animation.Play();
            _audio.Play();
        }
    }
}