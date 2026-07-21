namespace Kader.Infrastructure.Jwt
{
    public sealed class Token
    {
        public string AccessToken { get; }
        public string token { get; }
        public int ExpiresIn { get; }
        public Token(string token, int expiresIn)
        {
            this.token = token;
            ExpiresIn = expiresIn;
        }
    }
}
