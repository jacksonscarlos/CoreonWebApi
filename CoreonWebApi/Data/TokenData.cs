using CoreonWebApi.ProviderJWT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreonWebApi.Data
{
    public class TokenData
    {
        public static TokenJWT GravaToken(string usuario)
        {
            var token = new TokenJWTBuilder()
               .AddSecurityKey(ProviderJWT.JWTSecurityKey.Create("Secret_Key-12345678"))
               .AddSubject(usuario)
               .AddIssuer("Teste.Securiry.Bearer")
               .AddAudience("Teste.Securiry.Bearer")
               .AddClaim("UsuarioAPINumero", "1")
               .AddExpiry(5)
               .Builder();

            return token;
        }
    }
}
