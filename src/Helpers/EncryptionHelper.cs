namespace InsuraNova.Helpers
{
    public interface IEncryptionHelper
    {
        string Encrypt(string input);
    }
    public class EncryptionHelper : IEncryptionHelper
    {
        public string Encrypt(string input)
        {
            // Your encryption logic here
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(input));
        }

    }
}
