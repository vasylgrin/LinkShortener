namespace LinkShortener.Entity.Models
{
    public sealed class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public User() { }

        public User(string login, string password)
        {
            if (string.IsNullOrWhiteSpace(login))
            {
                throw new ArgumentException($"'{nameof(login)}' cannot be null or whitespace.", nameof(login));
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException($"'{nameof(password)}' cannot be null or whitespace.", nameof(password));
            }

            Login = login;
            Password = password;
        }
    }
}
