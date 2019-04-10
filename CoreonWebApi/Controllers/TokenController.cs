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
using CoreonWebApi.ProviderJWT;
using Microsoft.AspNetCore.Authorization;
using CoreonWebApi.Data;

namespace CoreonWebApi.Controllers
{
    public class TokenController : Controller
    {
        [Route("api/CreateToken")]
        [AllowAnonymous]
        [HttpPost]
        [Produces("application/json")]

        public IActionResult CreateToken([FromBody] Usuario user)
        {
            if (Autenticado(user.Username, user.Password))
                return Ok(TokenData.GravaToken(user.Username));
            else
                return Unauthorized("Usuario não autenticado");
        }

        private bool Autenticado(string usuario, string senha)
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

        }

    //    public class TokenController : Controller
    //{

    //    [Route("api/CreateToken")]
    //    [AllowAnonymous]
    //    [HttpPost]
    //    [Produces("application/json")]
    //    public IActionResult CreateToken([FromBody] Usuario user)
    //    {
    //        if (user.Username != "valdir" || user.Password != "1234")
    //            return Unauthorized();

    //        var token = new TokenJWTBuilder()
    //            .AddSecurityKey(ProviderJWT.JWTSecurityKey.Create("Secret_Key-12345678"))
    //            .AddSubject("Valdir Ferreira")
    //            .AddIssuer("Teste.Securiry.Bearer")
    //            .AddAudience("Teste.Securiry.Bearer")
    //            .AddClaim("UsuarioAPINumero", "1")
    //            .AddExpiry(5)
    //            .Builder();

    //        return Ok(token.value);
    //    }
    //}
}
