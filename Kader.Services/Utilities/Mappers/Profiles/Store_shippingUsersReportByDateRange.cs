using AutoMapper;
using Kader.Data.DataAccessLayer.Entities;
using Kader.Data.DataAccessLayer.Entities.StoredProcedure;
using Kader.DTOs.Store_shippingUsersReportByDateRange;
using Kader.DTOs.Product;
using Kader.Infrastructure.Shared.Implementations;
using Kader.Infrastructure.Shared.Interfaces;

namespace Kader.Services.Utilities.Mappers.Profiles
{
 
    public class Store_shippingUsersReportByDateRangeProfile : Profile
    {
        private IMethods _methods;
        public Store_shippingUsersReportByDateRangeProfile()
        {
            _methods = new Methods();
            CommandMapper();
            QueryMapper();
        }

        private void CommandMapper()
        {
            CreateMap<Store_shippingUsersReportByDateRangeDto, Store_shippingUsersReportByDateRange>();

        }
        private void QueryMapper()
        {
            CreateMap<Store_shippingUsersReportByDateRange, Store_shippingUsersReportByDateRangeDto>(); 
        }
    }

}
