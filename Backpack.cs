namespace BackpackApp
{
    public class Backpack
    {
        public string Color { get; private set; }
        public string Brand { get; private set; }
        public string Fabric { get; private set; }
        public double Weight { get; private set; }
        public double MaxVolume { get; private set; }

        public List<BackpackItem> Contents { get; private set; }

        public event Action<BackpackItem> ItemAdded;
        public event Action<BackpackItem> ItemRemoved;
        public event Action<BackpackItem, double> ItemChanged;

        public Backpack()
        {
            Contents = new List<BackpackItem>();
        }
        public void SetCharacteristics(string color, string brand, string fabric, double weight, double maxVolume)
        {
            Color = color;
            Brand = brand;
            Fabric = fabric;
            Weight = weight;
            MaxVolume = maxVolume;
        }

        public double GetCurrentVolume()
        {
            return Contents.Sum(item => item.Volume);
        }

        public void AddItem(BackpackItem item)
        {
            if (GetCurrentVolume() + item.Volume > MaxVolume)
            {
                throw new BackpackOverflowException($"Cannot add '{item.Name}'. Backpack volume exceeded!");
            }
            Contents.Add(item);

            ItemAdded?.Invoke(item);
        }

        public void RemoveItem(BackpackItem item)
        {
            if (Contents.Remove(item))
            {
                ItemRemoved?.Invoke(item);
            }
        }
        public void ChangeItemVolume(BackpackItem item, double newVolume)
        {
            if (!Contents.Contains(item)) return;

            double volumeDifference = newVolume - item.Volume;

            if (GetCurrentVolume() + volumeDifference > MaxVolume)
            {
                throw new BackpackOverflowException($"Cannot change volume for '{item.Name}'. This will exceed the backpack's volume!");
            }

            double oldVolume = item.Volume;
            item.Volume = newVolume;

            ItemChanged?.Invoke(item, oldVolume);
        }
    }
}
