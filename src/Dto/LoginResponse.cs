﻿namespace InsuraNova.Dto
{
    public class LoginResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public UserProfile User { get; set; }
    }
}
