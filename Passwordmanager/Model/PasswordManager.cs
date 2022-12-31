namespace Passwordmanager.Model
{
    public class PasswordManager
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string website { get; set; }
        public DateTime CreatedOn  { get; set; }
        public string? Encryptionkey { get; set; }

    }
}
