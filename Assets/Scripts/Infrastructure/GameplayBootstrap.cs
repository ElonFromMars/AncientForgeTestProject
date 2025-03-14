using AssetManagement.Prefabs;
using AssetManagement.Sprites;
using Configs.Common;
using Gameplay.Controllers.Common;
using Gameplay.Models.Interface;
using UnityEngine;

namespace Infrastructure
{
    public class GameplayBootstrap : MonoBehaviour
    {
        [SerializeField] private ConfigHolderSO configHolder;
        [SerializeField] private UIViewPrefabHolderSO uiViewPrefabHolder;
        [SerializeField] private SpriteHolderSO spriteHolder;
        [SerializeField] private MonoTicker monoTicker;
        
        private GameplaySceneController _gameplaySceneController;
        
        private void Awake()
        {
            _gameplaySceneController = new GameplaySceneController(
                configHolder,
                uiViewPrefabHolder,
                spriteHolder,
                monoTicker
            );
            _gameplaySceneController.Initialize();
        }
        
        private void OnDestroy()
        {
            _gameplaySceneController?.Dispose();
        }
    }
}