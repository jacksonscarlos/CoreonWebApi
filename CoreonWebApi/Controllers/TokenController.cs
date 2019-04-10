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
using ControleAcessoService;
using CoreonWebApi.Infra;
using System.Text;

namespace CoreonWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class TokenController : Controller
    {
        [HttpGet]
        public IActionResult Get([Required] string usuario, [Required] string senha)
        {
            if (Autenticado(usuario, senha))
            {
                return Ok(GravaToken(usuario));
            }
            else
                return Unauthorized("Usuario não autenticado");
        }

        private bool Autenticado (string usuario, string senha)
        {
            ControleAcessoServiceClient servico = new ControleAcessoServiceClient();

            try
            {
                var resultService = servico.ListarUsuarioPorLoginAsync(usuario);

                resultService.Wait();

                if (HashEncryption.Compare(senha.ToUpper(), resultService.Result.Senha.ToUpper()) && (resultService.Result.Status.Equals("A")))
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private string GravaToken(string usuario)
        {

            var clainsData = new[] { new Claim(ClaimTypes.Name, usuario) };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("CoreonSecurityKey"));
            var singInCred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var token = new JwtSecurityToken(
            issuer: "www.coreon.com.br",
            audience: "www.coreon.com.br",
            expires: DateTime.Now.AddMinutes(10),
            claims: clainsData,
            signingCredentials: singInCred);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenString;
        }
    }
}
