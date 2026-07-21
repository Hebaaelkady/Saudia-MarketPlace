using AutoMapper;
using Kader.Data.DataAccessLayer.Entities;
using Kader.Data.DataAccessLayer.Entities.StoredProcedure;
using Kader.DTOs.Stores;
using Kader.DTOs.Product;
using Kader.Infrastructure.Shared.Implementations;
using Kader.Infrastructure.Shared.Interfaces;

namespace Kader.Services.Utilities.Mappers.Profiles
{
 
    public class StoresProfile : Profile
    {
        private IMethods _methods;
        public StoresProfile()
        {
            _methods = new Methods();
            CommandMapper();
            QueryMapper();
        }

        private void CommandMapper()
        {
            CreateMap<StoresDto, Stores>();

        }
        private void QueryMapper()
        {
            CreateMap<Stores, StoresDto>();
            CreateMap<Stores, StoreDto>();
        }
    }

}
