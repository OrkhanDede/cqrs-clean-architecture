namespace Core.Models
{
    public class AesCryptoSettings
    {
        public string Salt { get; set; }
        public string Iv { get; set; }
        public string Key { get; set; }
    }
}