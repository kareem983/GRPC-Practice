namespace Ordering.Inventory.Data
{
    public class Database
    {
        public List<Item> GetItems()
        {
            return new List<Item>
            {
                new()
                {
                    Id = 1,
                    Quantity = 10
                },

                new()
                {
                    Id = 2,
                    Quantity = 20
                },

                new()
                {
                    Id = 3,
                    Quantity = 30
                },

                new()
                {
                    Id = 4,
                    Quantity = 40
                },

                new()
                {
                    Id = 5,
                    Quantity = 50
                },
            };

        }
    }
}
