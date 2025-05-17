namespace Ordering.Payment.Data
{
    public class Database
    {
        public List<User> GetUsers()
        {
            return new List<User>
            {
                new()
                {
                    Id = 1,
                    Balance = 1000
                },

                new()
                {
                    Id = 2,
                    Balance = 2000
                },

                new()
                {
                    Id = 3,
                    Balance = 3000
                },

                new()
                {
                    Id = 4,
                    Balance = 4000
                },

                new()
                {
                    Id = 5,
                    Balance = 5000
                },
            };
        }
    }
}
