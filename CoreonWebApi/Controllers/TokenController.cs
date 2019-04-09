using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using CoreonWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CoreonWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class TokenController : Controller
    {
        [HttpGet]
        public ActionResult<string> Get([Required] string usuario, [Required] string senha)
        {
            if (Autenticado(usuario, senha))
            {
                ClaimsIdentity identity = new ClaimsIdentity(
                        new GenericIdentity(usuario, "Login"),
                        new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, usuario)
                        }
                    );

                DateTime dataCriacao = DateTime.Now;
                DateTime dataExpiracao = dataCriacao +
                    TimeSpan.FromSeconds(600);

                var handler = new JwtSecurityTokenHandler();
                var securityToken = handler.CreateToken(new SecurityTokenDescriptor
                {
                    Issuer = "Coreon",
                    Audience = "Coreon",
                    //SigningCredentials = SymmetricSecurityKey
                    Subject = identity,
                    NotBefore = dataCriacao,
                    Expires = dataExpiracao
                });
                var token = handler.WriteToken(securityToken);

                return Ok(token);
            }
            else
            {
                return Unauthorized("Usuario não autenticado");
            }
        }

        private bool Autenticado (string usuario, string senha)
        {
            return true;
        }
    }
}
