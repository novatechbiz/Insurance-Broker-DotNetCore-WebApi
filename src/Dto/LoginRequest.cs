namespace InsuraNova.Dto
{
    public class LoginRequest : IRequest<LoginResponse>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; } = false;
    }
}
