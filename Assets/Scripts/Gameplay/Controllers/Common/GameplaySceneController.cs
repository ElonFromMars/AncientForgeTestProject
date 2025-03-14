using AssetManagement.Prefabs;
using AssetManagement.Sprites;
using Configs.Common;
using Gameplay.Controllers.Features;
using Gameplay.Models.Features.Bonuses.Services;
using Gameplay.Models.Features.Crafting.Services;
using Gameplay.Models.Features.Inventory.Services;
using Gameplay.Models.Features.Machines.Services;
using Gameplay.Models.Features.Quests.Services;
using Gameplay.Models.Interface;
using Gameplay.Views.UI.Common;

namespace Gameplay.Controllers.Common
{
    public class GameplaySceneController
    {
        private readonly ConfigHolderSO _configHolder;
        private readonly UIViewPrefabHolderSO _uiViewPrefabHolder;
        private readonly SpriteHolderSO _spriteHolder;
        private readonly ITicker _ticker;
        
        private InventoryService _inventoryService;
        private MachineService _machineService;
        private RecipeService _recipeService;
        private QuestService _questService;
        private CraftingService _craftingService;
        private BonusService _bonusService;
        
        private InventoryController _inventoryController;
        private MachinesController _machinesController;
        private QuestController _questController;
        private HudView _hudView;

        public GameplaySceneController(
            ConfigHolderSO configHolder,
            UIViewPrefabHolderSO uiViewPrefabHolder,
            SpriteHolderSO spriteHolder,
            ITicker ticker,
            HudView hudView
            )
        {
            _hudView = hudView;
            _configHolder = configHolder;
            _uiViewPrefabHolder = uiViewPrefabHolder;
            _spriteHolder = spriteHolder;
            _ticker = ticker;
        }

        public void Initialize()
        {
            InitializeServices();
            InitializeControllers();
            
            _ticker.OnTick += OnTick;
        }

        public void Dispose()
        {
            _ticker.OnTick -= OnTick;

            DisposeServices();
        }

        private void OnTick(float deltaTime)
        {
            _craftingService.Update(deltaTime);
            _machinesController.Update(deltaTime);
        }

        private void InitializeServices()
        {
            _inventoryService = new InventoryService(_configHolder);
            _machineService = new MachineService(_configHolder);
            _recipeService = new RecipeService(_configHolder);
            _questService = new QuestService(_configHolder.QuestConfigHolder);
            _craftingService = new CraftingService(_recipeService, _machineService, _inventoryService);
            _bonusService = new BonusService(_configHolder, _inventoryService, _machineService);
            
            _recipeService.Initialize();
            _inventoryService.InitializeStartingInventory();
            _bonusService.Initialize();
            _questService.Initialize();
        }

        private void DisposeServices()
        {
            _inventoryController.Dispose();
            _machinesController.Dispose();
            _questController.Dispose();
            _bonusService.Dispose();
        }

        private void InitializeControllers()
        {
            _inventoryController = new InventoryController(
                _inventoryService,
                _spriteHolder,
                _uiViewPrefabHolder,
                _configHolder.ItemConfigHolder,
                _hudView
            );
            
            _machinesController = new MachinesController(
                _machineService,
                _recipeService,
                _uiViewPrefabHolder,
                _spriteHolder,
                _configHolder.ItemConfigHolder,
                _configHolder.RecipeConfigHolder,
                _configHolder.MachineConfigHolder,
                _hudView
            );
            
            _questController = new QuestController(
                _questService,
                _uiViewPrefabHolder,
                _configHolder.QuestConfigHolder,
                _hudView
            );
            
            _inventoryController.Initialize();
            _machinesController.Initialize();
            _questController.Initialize();
        }
    }
}