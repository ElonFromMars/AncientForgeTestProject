## TestProject for AncientForge

### Architecture
The project uses a simple architecture based on the MVC approach. Models are allocated in such a way as not to depend on the Unity API, which will simplify further development of the project (serialization, synchronization, simulation, networking).

### Plugins
From the plugins, only tech mesh pro. For clear text rendering.

Coroutines and UniTask will not be used. To avoid being tied to a specific approach.

### Implementation
To reduce coupling, standard C# Actions are used.
The entire project will start with GameplayBootstrap.cs Further, controllers will be responsible for creating models from configs and linking them to the corresponding view.

### Subsystems
- Inventory
- Machines
- Crafting
- Quests
- Bonuses

### Installation
Unity version: [2022.3.5f1](https://unity.com/releases/editor/whats-new/2022.3.5) or higher
