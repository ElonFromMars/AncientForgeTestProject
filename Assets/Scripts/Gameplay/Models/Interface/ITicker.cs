using System;

namespace Gameplay.Models.Interface
{
    public interface ITicker
    {
        public Action<float> OnTick { get; set; }
    }
}