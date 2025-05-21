namespace InsuraNova.Configurations
{ 
    public class EmailConfig
    {
        public bool Pool { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool Secure { get; set; }
        public TlsConfig Tls { get; set; }
        public AuthConfig Auth { get; set; }
        public string Region { get; set; }


        public class TlsConfig
        {
            public bool RejectUnauthorized { get; set; }
        }

        public class AuthConfig
        {
            public string User { get; set; }
            public string Pass { get; set; }
        }
    }
}
