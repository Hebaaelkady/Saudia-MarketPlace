namespace Kader.Infrastructure.Jwt
{
    public static class Constants
    {
        public static class Strings
        {
            public static class JwtClaimIdentifiers
            {
                public const string UserName = "userName", Id = "id", FirebaseToken = "FB_Token;",RoleId = "RoleId",PersonId = "PersonId";
            }

            public static class JwtClaims
            {
                public const string ApiAccess = "api_access";
            }
        }
    }
}
