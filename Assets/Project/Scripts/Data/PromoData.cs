namespace RedPanda.Project.Data
{
    public class PromoData
    {
        public PromoType Type { get; }
        public string Title { get; }
        public PromoRarity Rarity { get; }
        public int Cost { get; }

        public string Icon()
        {
            return $"Sprites/sprite_{Type.ToString().ToLower()}_{Rarity.ToString().ToLower()}";
        }

        public string Background()
        {
            return $"Sprites/background_{Rarity.ToString().ToLower()}";
        }
        
        public PromoData(string title, PromoType type, PromoRarity rarity, int cost)
        {
            Title = title;
            Type = type;
            Rarity = rarity;
            Cost = cost;
        }
    }

    public enum PromoRarity
    {
        Common,
        Rare,
        Epic
    }

    public enum PromoType
    {
        Chest,
        Special,
        InApp
    }
}