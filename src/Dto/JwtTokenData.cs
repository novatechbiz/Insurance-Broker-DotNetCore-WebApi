namespace InsuraNova.Dto
{
    public class JwtTokenData
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public int CompanyId { get; set; }
        public string Email { get; set; }
    }
}
