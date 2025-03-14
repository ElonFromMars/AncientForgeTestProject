using System.Collections.Generic;
using System.Linq;
using AssetManagement.Configs;
using UnityEngine;

namespace AssetManagement.Prefabs
{
    [CreateAssetMenu(fileName = nameof(UIViewPrefabHolderSO), menuName = "ScriptableObjects/" + nameof(UIViewPrefabHolderSO), order = 1)]
    public class UIViewPrefabHolderSO : ScriptableObject
    {
        [SerializeField] private List<UIPrefabViewEntry> entries = new List<UIPrefabViewEntry>();
        
        public GameObject GetPrefab(UIViewId viewId)
        {
            var entry = entries.FirstOrDefault(e => e.ViewId == viewId);
            
            if (entry != null)
            {
                return entry.Prefab;
            }
            
            Debug.LogWarning($"Prefab for UI view {viewId} not found!");
            return null;
        }
    }
}