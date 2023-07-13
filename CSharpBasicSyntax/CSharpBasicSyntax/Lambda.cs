namespace CSharpBasicSyntax
{
    public enum ItemType
    { 
        Weapon,
        Armor,
        Amulet,
        Ring
    }

    public enum Rarity
    { 
        Normal,
        Uncommon,
        Rare
    }

    public class Item
    {
        public ItemType ItemType;
        public Rarity Rarity;
    }

    internal class Lambda
    {
        static List<Item> items = new List<Item>();

        delegate bool ItemSelector(Item item);

        static Item FindItem(ItemSelector selector)
        {
            for (int i = 0; i < items.Count; i++)
                if (selector(items[i]))
                    return items[i];

            return null;
        }

        //static void Main(string[] args)
        //{
        //    items.Add(new Item() { ItemType = ItemType.Weapon, Rarity = Rarity.Normal });
        //    items.Add(new Item() { ItemType = ItemType.Armor, Rarity = Rarity.Uncommon });
        //    items.Add(new Item() { ItemType = ItemType.Ring, Rarity = Rarity.Rare });

        //    Item item = FindItem(item => item.ItemType == ItemType.Weapon);
        //    Console.WriteLine(item.ItemType);
        //}
    }
}
