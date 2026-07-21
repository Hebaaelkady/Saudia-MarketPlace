using AutoMapper;
using Kader.Data.DataAccessLayer.Entities;
using Kader.Data.DataAccessLayer.Entities.StoredProcedure;
using Kader.DTOs.CatType;
using Kader.DTOs.Colors;
using Kader.DTOs.Product;
using Kader.DTOs.BannerImgs;
using Kader.Infrastructure.Shared.Implementations;
using Kader.Infrastructure.Shared.Interfaces;

namespace Kader.Services.Utilities.Mappers.Profiles
{
 
    public class BannerImgsProfile : Profile
    {
        private IMethods _methods;
        public BannerImgsProfile()
        {
            _methods = new Methods();
            CommandMapper();
            QueryMapper();
        }

        private void CommandMapper()
        {
            CreateMap<BannerImgsDto, BannerImgs>();
             

        }
        private void QueryMapper()
        {
            CreateMap<BannerImgs, BannerImgsDto>() ;
            
        }
    }

}
