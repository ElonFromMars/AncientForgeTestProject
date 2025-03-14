using System;
using UnityEngine;

namespace AssetManagement.Sprites
{
    [Serializable]
    public class SpriteEntry
    {
        [SerializeField] private SpriteId spriteId;
        [SerializeField] private Sprite sprite;
        
        public SpriteId SpriteId => spriteId;
        public Sprite Sprite => sprite;
    }
}
