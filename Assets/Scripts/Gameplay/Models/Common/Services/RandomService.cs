using System;

namespace Gameplay.Models.Common.Services
{
    public class RandomService
    {
        System.Random _random = new Random();

        public float GetRandomFloat(float min, float max)
        {
            return (float)_random.NextDouble() * (max - min) + min;
        }
    }
}