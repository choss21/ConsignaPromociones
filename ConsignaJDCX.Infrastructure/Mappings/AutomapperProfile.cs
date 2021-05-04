using AutoMapper;
using ConsignaJDCX.Core.DTOS;
using ConsignaJDCX.Core.Entities;

namespace ConsignaJDCX.Infrastructure.Mappings
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<BaseEntityDTO, BaseEntity>()
                .Include<PromotionDTO, Promotion>();
            CreateMap<PromotionDTO, Promotion>();

            CreateMap<BaseEntity, BaseEntityDTO>()
                .Include<Promotion, PromotionDTO>();
            CreateMap<Promotion, PromotionDTO>();

        }

    }
}
