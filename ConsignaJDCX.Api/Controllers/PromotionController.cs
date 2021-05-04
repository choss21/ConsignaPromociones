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
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="promotionService"></param>
        /// <param name="mapper"></param>
        public PromotionController(IPromotionService promotionService, IMapper mapper)
        {
            _promotionService = promotionService;
            _mapper = mapper;
        }
        /// <summary>
        /// 1. Ver listado de promociones
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<PromotionDTO>>> GetPromotions()
        {
            try
            {
                var result = await _promotionService.GetPromotions();
                var dtos = _mapper.Map<List<PromotionDTO>>(result);
                return dtos;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// 2. Ver una promoción
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<PromotionDTO>> GetPromotion(Guid id)
        {
            try
            {
                var result = await _promotionService.GetPromotion(id);
                var dto = _mapper.Map<PromotionDTO>(result);
                return dto;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Ver listado de promociones vigentes para una fecha x
        /// </summary>
        /// <returns></returns>
        [HttpGet("availables")]
        public async Task<ActionResult<List<PromotionDTO>>> GetAvailablePromotions()
        {
            try
            {
                var result = await _promotionService.GetAvailablePromotions(DateTime.Now);
                var dtos = _mapper.Map<List<PromotionDTO>>(result);
                return dtos;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Ver listado de promociones vigentes para una fecha x
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpGet("availables/{date}")]
        public async Task<ActionResult<List<PromotionDTO>>> GetAvailablePromotionsByDate(DateTime date)
        {
            try
            {
                var result = await _promotionService.GetAvailablePromotions(date);
                var dtos = _mapper.Map<List<PromotionDTO>>(result);
                return dtos;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Para crear una Promocion
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<PromotionDTO>> Create([FromBody] SaveRequest<PromotionDTO> request)
        {
            try
            {
                if (request == null)
                    throw new ArgumentNullException(nameof(request));
                if (request.Entity == null)
                    throw new ArgumentNullException(nameof(request.Entity));

                var p = _mapper.Map<Promotion>(request.Entity);
                await _promotionService.CreatePromotion(p);
                var dto = _mapper.Map<PromotionDTO>(p);
                return dto;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<PromotionDTO>> ModifyPromotion([FromBody] SaveRequest<PromotionDTO> request)
        {

            try
            {
                if (request == null)
                    throw new ArgumentNullException(nameof(request));
                if (request.EntityId == null)
                    throw new ArgumentNullException("Se requiere EntityId");
                if (request.Entity == null)
                    throw new ArgumentNullException(nameof(request.Entity));
                var p = _mapper.Map<Promotion>(request.Entity);
                p = await _promotionService.UpdatePromotion(request.EntityId.Value, p);
                var dto = _mapper.Map<PromotionDTO>(p);
                return dto;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Modificar vigencia de promocion
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("validity")]
        public async Task<ActionResult<PromotionDTO>> ModifyValidityPromotion([FromBody] SaveRequest<PromotionOnlyValidityDTO> request)
        {
            try
            {
                if (request == null)
                    throw new ArgumentNullException(nameof(request));
                if (request.EntityId == null)
                    throw new ArgumentNullException("Se requiere EntityId");
                if (request.Entity == null)
                    throw new ArgumentNullException(nameof(request.Entity));

                var p = await _promotionService.UpdateValidatePromotion(request.EntityId.Value, request.Entity.FechaInicio, request.Entity.FechaFin);
                var dto = _mapper.Map<PromotionDTO>(p);
                return dto;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Eliminar una promoción
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                await _promotionService.DeletePromotion(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
