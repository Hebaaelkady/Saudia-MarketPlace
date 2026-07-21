using AutoMapper;
using Kader.Data.DataAccessLayer.Entities;
using Kader.Data.DataAccessLayer.Entities.StoredProcedure;
using Kader.DTOs.LogQuantity;
using Kader.DTOs.Product;
using Kader.Infrastructure.Shared.Implementations;
using Kader.Infrastructure.Shared.Interfaces;

namespace Kader.Services.Utilities.Mappers.Profiles
{
 
    public class LogQuantityProfile : Profile
    {
        private IMethods _methods;
        public LogQuantityProfile()
        {
            _methods = new Methods();
            CommandMapper();
            QueryMapper();
        }

        private void CommandMapper()
        {
            CreateMap<LogQuantityDto, LogQuantity>();

        }
        private void QueryMapper()
        {
            CreateMap<LogQuantity, LogQuantityDto>(); 
        }
    }

}
