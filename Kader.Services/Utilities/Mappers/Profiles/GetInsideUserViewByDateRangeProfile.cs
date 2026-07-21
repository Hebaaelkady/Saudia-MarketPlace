using AutoMapper;
using Kader.Data.DataAccessLayer.Entities;
using Kader.Data.DataAccessLayer.Entities.StoredProcedure;
using Kader.DTOs.Address;
using Kader.DTOs.GetInsideUserViewByDateRange;
using Kader.DTOs.Product;
using Kader.Infrastructure.Shared.Implementations;
using Kader.Infrastructure.Shared.Interfaces;

namespace Kader.Services.Utilities.Mappers.Profiles
{
 
    public class GetInsideUserViewByDateRangeProfile : Profile
    {
        private IMethods _methods;
        public GetInsideUserViewByDateRangeProfile()
        {
            _methods = new Methods();
            CommandMapper();
            QueryMapper();
        }

        private void CommandMapper()
        {
            CreateMap<GetInsideUserViewByDateRangeDto, GetInsideUserViewByDateRange>();

        }
        private void QueryMapper()
        {
            CreateMap<GetInsideUserViewByDateRange, GetInsideUserViewByDateRangeDto>(); 
        }
    }

}
