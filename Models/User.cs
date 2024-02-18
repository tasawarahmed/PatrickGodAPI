namespace PatrickGodAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; } = new byte[0];
        public byte[] PasswordSalt { get; set; } = new byte[0];
        //Implementing one to many relationship. One user can have many characters
        public List<Character>? Characters { get; set; }
    }
}
