using System;
using Gameplay.Models.Interface;
using UnityEngine;

namespace Infrastructure
{
    public class MonoTicker : MonoBehaviour, ITicker
    {
        public Action<float> OnTick { get; set; }

        private void Update()
        {
            float deltaTime = Time.deltaTime;
            OnTick?.Invoke(deltaTime);
        }
    }
}