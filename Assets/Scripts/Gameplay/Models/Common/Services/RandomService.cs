using System;

namespace Gameplay.Models.Common.Services
{
    public class RandomService
    {
        private System.Random _random = new Random();

        public float GetRandomFloat(float min, float max)
        {
            return (float)_random.NextDouble() * (max - min) + min;
        }

        public int GetRandomInt(int itemConfigMinAmount, int itemConfigMaxAmount)
        {
            return _random.Next(itemConfigMinAmount, itemConfigMaxAmount + 1);
        }
    }
}