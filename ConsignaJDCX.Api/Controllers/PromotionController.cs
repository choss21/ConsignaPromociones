using ConsignaJDCX.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsignaJDCX.Api.Controllers
{
    /// <summary>
    /// WebApi para obtener informacion sobre promociones
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionController : ControllerBase
    {
        /// <summary>
        /// Obtiene un listado de promociones
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<Promotion>>> GetPromotions()
        {
            return null;

        }
    }
}
