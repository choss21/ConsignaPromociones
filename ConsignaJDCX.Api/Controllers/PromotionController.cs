using AutoMapper;
using ConsignaJDCX.Core.DTOS;
using ConsignaJDCX.Core.Entities;
using ConsignaJDCX.Core.Interfaces;
using ConsignaJDCX.Core.Messages;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
        private readonly IMapper _mapper;
        public PromotionController(IPromotionService promotionService, IMapper mapper)
        {
            _promotionService = promotionService;
            _mapper = mapper;
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
        /// <summary>
        /// Para crear una Promocion
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Promotion>> Create([FromBody] SaveRequest<PromotionDTO> request)
        {
            try
            {
                if (request == null)
                    throw new ArgumentNullException(nameof(request));
                if (request.Entity == null)
                    throw new ArgumentNullException(nameof(request.Entity));

                var p = _mapper.Map<Promotion>(request.Entity);
                await _promotionService.CreatePromotion(p);
                return p;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
