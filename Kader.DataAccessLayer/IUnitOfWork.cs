using Kader.Data.DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using Kader.Data.DataAccessLayer.Repositories.Interfaces;

namespace Kader.Data.DataAccessLayer
{
    public interface IUnitOfWork : IDisposable
    {
        #region Tables

        IGetInsideUserViewByDateRange_sp GetInsideUserViewByDateRange { get; }
        IStore_shippingUsersReportByDateRange_sp Store_shippingUsersReportByDateRange { get; }
        IPaymentRepository Payment { get; }
        IAdsRepository Ads { get; }
        IStoreShippingUsersReportRepository StoreShippingUsersReport { get; }
        ILogQuantityRepository LogQuantity { get; }
        IStoresRepository Stores { get; }
        IInsideUserViewRepository InsideUserView { get; }
        IUsersStoresRepository UsersStores { get; }
        IReturnsOrderItemRepository ReturnsOrderItem { get; }
        IReturnsOrderStatusRepository ReturnsOrderStatus { get; }
        IReturnsReasonRepository ReturnsReason { get; }
        IReturnsOrderRepository ReturnsOrder { get; }
        ICatogryRepository Catogry { get; }
        IPagesRepository Pages { get; }
        IApplicationRoleRepository ApplicationRole { get; } 
        IApplicationUserRepository ApplicationUser { get; }
        ILogPriceRepository LogPrice { get; }
        IBannerImgsRepository BannerImgs { get; }
        IGovernoratesRepository Governorates { get; }
        ICitiesRepository Cities { get; }
        INeighborhoodRepository Neighborhood { get; }
        IOrderStatusRepository OrderStatus { get; }
        IProductRepository Product { get; }
        ICatTypeRepository CatType { get; }
        IUnitsRepository Units { get; }
        IColorsRepository Colors { get; }
        IOrderssRepository Orderss { get; }
        IOrderItemsRepository OrderItems { get; }
        IAddressRepository Address { get; }
        IProductImgRepository ProductImg { get; }
        IShippingPriceRepository ShippingPrice { get; }
        IRoleDetailRepository RoleDetail { get; }
        #endregion
        int Complete();
        Task<int> CompleteAsync();
        Task<List<T>> ToListAsync<T>(IQueryable<T> query);
        Task<IEnumerable<T>> ToEnumerable<T>(IQueryable<T> query);
        Task<T[]> ToArrayAsync<T>(IQueryable<T> query);
        Task<int> CountQuery<T>(IQueryable<T> query);
        Task<T> ToSingleItem<T>(IQueryable<T> query);
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}