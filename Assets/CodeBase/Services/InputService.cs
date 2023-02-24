// Copyright (c) 2012-2021 FuryLion Group. All Rights Reserved.

using System;
using UnityEngine;

namespace CodeBase.Services
{
    public class InputService : MonoBehaviour
    {
        public event Action<Vector3> Press;
        public event Action PressUp;
        private void Update()
        {
        
            if (Input.GetMouseButton(0))
            {
                Press?.Invoke(Input.mousePosition);
            }

            if (Input.GetMouseButtonUp(0))
            {
                PressUp?.Invoke();
            }
        }
    
    }
}