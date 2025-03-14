﻿## TestProject for AncientForge

### Architecture
The project uses a simple architecture based on the MVC approach. [Models](Assets/Scripts/Gameplay/Models) are allocated in such a way as not to depend on the Unity API, which will simplify further development of the project (serialization, synchronization, simulation, networking).

### Plugins
From the plugins, only tech mesh pro. For clear text rendering.

Coroutines and UniTask will not be used. To avoid being tied to a specific approach.

### Implementation
To reduce coupling, standard C# Actions are used.
The entire project will start with [GameplayBootstrap.cs](Assets/Scripts/Infrastructure/GameplayBootstrap.cs) Further, controllers will be responsible for creating models from configs and linking them to the corresponding views.
Configs can be found in the config holder in the [Assets/Configs](Assets/Configs) folder.
Sprites and prefabs are also loaded from the holders to add flexibility in the future.

### Features
- Inventory
- Machines
- Crafting
- Quests
- Bonuses

### Installation
Unity version: [2022.3.5f1](https://unity.com/releases/editor/whats-new/2022.3.5) or higher
