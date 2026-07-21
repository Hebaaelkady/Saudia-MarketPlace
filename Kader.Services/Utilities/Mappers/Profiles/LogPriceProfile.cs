using AutoMapper;
using Kader.Data.DataAccessLayer.Entities;
 
using Kader.DTOs.LogPrice;
using Kader.Infrastructure.Shared.Implementations;
using Kader.Infrastructure.Shared.Interfaces;

namespace Kader.Services.Utilities.Mappers.Profiles
{
 
    public class LogPriceProfile : Profile
    {
        private IMethods _methods;
        public LogPriceProfile()
        {
            _methods = new Methods();
            CommandMapper();
            QueryMapper();
        }

        private void CommandMapper()
        {
            CreateMap<LogPriceDto, LogPrice>().ForMember(dis => dis.DiscountEndDate, map => map.MapFrom(sourse => sourse.discountEndDat)); 

        }
        private void QueryMapper()
        {
            CreateMap<LogPrice, LogPriceDto>()
                .ForMember(dis => dis.AfterDiscount, map => map.MapFrom(sourse => sourse.AfterDiscount))
             .ForMember(dis => dis.discountEndDat, map => map.MapFrom(sourse => sourse.DiscountEndDate));
        }
    }

}
