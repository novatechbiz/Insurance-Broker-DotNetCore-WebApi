namespace InsuraNova.Helpers
{
    public static class ApiStatus
    {
        public const string Success = "success";
        public const string Failure = "failure";
    }

    public static class ApiAction
    {
        public const string Created = "created";
        public const string Updated = "updated";
        public const string Read = "read";
        public const string Deleted = "deleted";
        public const string Authenticated = "authenticated";
    }

    public static class RecordStatus
    {
        public const int Pending = 0;
        public const int Active = 1;
        public const int Deleted = -1;
        public const int Completed = 2;
        public const int ErrorRow = -10;
        public const int DuplicateRow = -5;
    }

    public static class UserRoleIds
    {
        public const int Admin = 1;
        public const int CompanyAdmin = 2;
        public const int ProjectAdmin = 3;
        public const int Player = 4;
        public const int Inspector = 5;
        public const int Analyzer = 6;
    }

    public static class DefaultSettings
    {
        public const string DefaultSecret = "SedaragiRo25!";
        public const int ResetPasswordExpiryInMinutes = 1;
    }
  
}
