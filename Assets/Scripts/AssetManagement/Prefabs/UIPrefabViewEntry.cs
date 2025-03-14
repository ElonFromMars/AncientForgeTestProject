using System;
using AssetManagement.Configs;
using UnityEngine;

namespace AssetManagement.Prefabs
{
    [Serializable]
    public class UIPrefabViewEntry
    {
        [SerializeField] private UIViewId viewId;
        [SerializeField] private GameObject prefab;
        
        public UIViewId ViewId => viewId;
        public GameObject Prefab => prefab;
    }
}
