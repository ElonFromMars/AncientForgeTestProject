using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AssetManagement.Sprites
{
    [CreateAssetMenu(fileName = nameof(SpriteHolderSO), menuName = "ScriptableObjects/" + nameof(SpriteHolderSO), order = 2)]
    public class SpriteHolderSO : ScriptableObject
    {
        [SerializeField] private List<SpriteEntry> entries = new List<SpriteEntry>();
        
        public Sprite GetSprite(SpriteId spriteId)
        {
            var entry = entries.FirstOrDefault(e => e.SpriteId == spriteId);
            
            if (entry != null)
            {
                return entry.Sprite;
            }
            
            Debug.LogWarning($"Sprite for ID {spriteId} not found!");
            return null;
        }
    }
}