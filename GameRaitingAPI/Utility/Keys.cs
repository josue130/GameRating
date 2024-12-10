using Microsoft.IdentityModel.Tokens;
using static System.Collections.Specialized.BitVector32;

namespace GameRaitingAPI.Utility
{
    public class Keys
    {
        public const string Issuer = "game-raiting";
        private const string Section = "Authentication:Schemes:Bearer:SigningKeys";
        private const string IssuerSection = "Issuer";
        private const string Secret = "Secret";

        public static IEnumerable<SecurityKey> GetSecret(IConfiguration configuration)
            => GetSecret(configuration, Issuer);

        public static IEnumerable<SecurityKey> GetSecret(IConfiguration configuration, string issuer)
        {
            var signingKey = configuration.GetSection(Section)
                .GetChildren()
                .SingleOrDefault(llave => llave[IssuerSection] == issuer);

            if (signingKey is not null && signingKey[Secret] is string SecretValue)
            {

                yield return new SymmetricSecurityKey(Convert.FromBase64String(SecretValue));
            }
        }
    }
    
}
