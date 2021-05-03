using ConsignaJDCX.Core.Entities;
using ConsignaJDCX.Core.Interfaces;
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
        private readonly IPromotionService _promotionService;
        public PromotionController(IPromotionService promotionService)
        {
            _promotionService = promotionService;
        }
        /// <summary>
        /// Obtiene un listado de promociones
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<Promotion>>> GetPromotions()
        {
            var result = await _promotionService.GetPromotions();

            return result;

        }

        [HttpPost]
        public async Task<ActionResult<Promotion>> Create([FromBody] Promotion s)
        {
            await _promotionService.CreatePromotion(s);
            return s;

        }
    }
}
