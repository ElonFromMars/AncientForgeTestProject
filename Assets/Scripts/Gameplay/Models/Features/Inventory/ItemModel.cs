namespace Gameplay.Models.Features.Inventory
{
    public class ItemModel
    {
        public ItemId Id { get; private set; }
        public string Name { get; private set; }
        public ItemType Type { get; private set; }
        public string Description { get; private set; }
        
        public ItemModel(ItemId id, string name, ItemType type, string description)
        {
            Id = id;
            Name = name;
            Type = type;
            Description = description;
        }
    }
}
