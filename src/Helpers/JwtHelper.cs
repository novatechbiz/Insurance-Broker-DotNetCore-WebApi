using System.IdentityModel.Tokens.Jwt;

namespace InsuraNova.Helpers
{
    public static class JwtHelper
    {

        public static bool TrySetPropertyFromToken<T>(HttpContext context, T entity, string propertyName, out IResult? errorResult) where T : class
        {
            var userEmail = GetEmailFromToken(context);
            if (userEmail != null)
            {
                var property = typeof(T).GetProperty(propertyName);
                if (property != null && property.PropertyType == typeof(string))
                {
                    property.SetValue(entity, userEmail);
                    errorResult = null;
                    return true;
                }
                else
                {
                    errorResult = Results.BadRequest($"{propertyName} property not found or not of type string.");
                    return false;
                }
            }
            else
            {
                errorResult = Results.BadRequest("User email not found in token.");
                return false;
            }
        }

        public static void InitializeEntityMetadata<T>(HttpContext context, T entity) where T : class
        {
            if (entity == null || context == null) return;

            var userEmail = GetEmailFromToken(context);
            if (string.IsNullOrEmpty(userEmail)) return;

            SetPropertyValue(entity, "CreatedBy", userEmail, typeof(string));
            SetPropertyValue(entity, "Created", DateTime.UtcNow, typeof(DateTime));
            SetPropertyValue(entity, "Version", 1, typeof(int));
            SetPropertyValue(entity, "RecordStatus", 1, typeof(int));
        }

        public static void InitializeUpdateMetadata<T>(HttpContext context, T entity) where T : class
        {
            if (entity == null || context == null) return;

            var userEmail = GetEmailFromToken(context);
            if (string.IsNullOrEmpty(userEmail)) return;

            SetPropertyValue(entity, "ModifiedBy", userEmail, typeof(string));
            SetPropertyValue(entity, "Modified", DateTime.UtcNow, typeof(DateTime));

        }

        private static void SetPropertyValue<T>(T entity, string propertyName, object value, Type propertyType) where T : class
        {
            var property = typeof(T).GetProperty(propertyName);
            if (property != null && property.PropertyType == propertyType)
            {
                property.SetValue(entity, value);
            }
        }


        public static string? GetEmailFromToken(HttpContext context)
        {
            // Get the token from the Authorization header
            var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            // Extract the user's email from the JWT token
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email);
            return emailClaim?.Value;
        }

        public static JwtTokenData GetJwtTokenData(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // Extract all required claims
            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "userId");
            var roleIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "roleId");
            var companyIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "companyId");
            var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email);

            // Construct the JwtTokenData object with the extracted claims
            return new JwtTokenData
            {
                UserId = userIdClaim != null ? int.Parse(userIdClaim.Value) : 0,
                RoleId = roleIdClaim != null ? int.Parse(roleIdClaim.Value) : 0,
                CompanyId = companyIdClaim != null ? int.Parse(companyIdClaim.Value) : 0,
                Email = emailClaim?.Value
            };

        }
    }
}
