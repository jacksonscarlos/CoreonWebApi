using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CoreonWebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoreonWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class TokenController : Controller
    {
        [HttpPost]
        public ActionResult<string> Post([Required] string usuario, [Required] string senha)
        {
            return Ok("Autorizado");


        }
    }
}
