namespace Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public static User Create(string name, string email, string password)
        {
            return new User
            {
                Name = name,
                Email = email,
                PasswordHash = password
            };
        }
    }
}
