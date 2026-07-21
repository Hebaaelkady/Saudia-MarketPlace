using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kader.Data.DataAccessLayer.Entities;
using Kader.Data.DataAccessLayer.Entities.StoredProcedure;
using Kader.Data.DataAccessLayer.Repositories.Implementations;
using Kader.Data.DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Kader.Data.DataAccessLayer
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DBContext _context;
        public IAdsRepository Ads { get; }


        public IApplicationRoleRepository ApplicationRole { get; }
        public IGetInsideUserViewByDateRange_sp GetInsideUserViewByDateRange { get; }
        public IStore_shippingUsersReportByDateRange_sp Store_shippingUsersReportByDateRange { get; }
        public IStoreShippingUsersReportRepository StoreShippingUsersReport { get; }
        public ILogQuantityRepository LogQuantity { get; }
        public IInsideUserViewRepository InsideUserView { get; }
        public IStoresRepository Stores { get; }
        public IUsersStoresRepository UsersStores { get; }
        public IReturnsOrderItemRepository ReturnsOrderItem { get; }
        public IReturnsReasonRepository ReturnsReason { get; }
        public IReturnsOrderStatusRepository ReturnsOrderStatus { get; }
        public IReturnsOrderRepository ReturnsOrder { get; }
        public IPagesRepository Pages { get; } 
        public IApplicationUserRepository ApplicationUser { get; }
        public ICatogryRepository Catogry { get; }
        public IPaymentRepository Payment { get; }
        public IAddressRepository Address { get; }
        public IOrderStatusRepository OrderStatus { get; }
        public ICitiesRepository Cities { get; }
        public INeighborhoodRepository Neighborhood { get; }
        public IBannerImgsRepository BannerImgs { get; }
        public IGovernoratesRepository Governorates { get; }
        public IProductRepository Product { get; }
        public ICatTypeRepository CatType { get; }
        public IUnitsRepository Units { get; }
        public IColorsRepository Colors { get; }
        public IOrderssRepository Orderss { get; }
        public IOrderItemsRepository OrderItems { get; }
        public IProductImgRepository ProductImg { get; }
        public IShippingPriceRepository ShippingPrice { get; }
        public ILogPriceRepository LogPrice { get; }
        public IRoleDetailRepository RoleDetail { get; }
        public UnitOfWork()
        {
            _context = new DBContext();
            GetInsideUserViewByDateRange = new GetInsideUserViewByDateRange_sp(_context);
            Store_shippingUsersReportByDateRange = new Store_shippingUsersReportByDateRange_sp(_context);

            StoreShippingUsersReport = new StoreShippingUsersReportRepository(_context);
            InsideUserView = new InsideUserViewRepository(_context);
            Ads = new AdsRepository(_context);
            LogQuantity = new LogQuantityRepository(_context);
            Pages = new PagesRepository(_context);
            UsersStores = new UsersStoresRepository(_context);
            Stores = new StoresRepository(_context);
            ReturnsOrderItem = new ReturnsOrderItemRepository(_context);
            OrderStatus = new OrderStatusRepository(_context);
            ReturnsReason = new ReturnsReasonRepository(_context);
            ReturnsOrderStatus = new ReturnsOrderStatusRepository(_context);
            ReturnsOrder = new ReturnsOrderRepository(_context);
            Address = new AddressRepository(_context);
            Payment = new PaymentRepository(_context);
            Catogry = new CatogryRepository(_context); BannerImgs = new BannerImgsRepository(_context);
            ApplicationRole = new ApplicationRoleRepository(_context); 
            ApplicationUser = new ApplicationUserRepository(_context);
            Product = new ProductRepository(_context);
            Governorates = new GovernoratesRepository(_context);
            CatType = new CatTypeRepository(_context);
            Neighborhood = new NeighborhoodRepository(_context);
            Cities = new CitiesRepository(_context);
            ProductImg = new ProductImgRepository(_context);
            ShippingPrice = new ShippingPriceRepository(_context);
            Units = new UnitsRepository(_context); OrderItems = new OrderItemsRepository(_context);
            Orderss = new OrderssRepository(_context);
            Colors = new ColorsRepository(_context);
            LogPrice = new LogPriceRepository(_context);
            RoleDetail = new RoleDetailRepository(_context);
        }
        public void Dispose()
        {
            _context.Dispose();
        }
        public int Complete()
        {
            var effectedRows = _context.SaveChanges();
            return effectedRows;
        }
        public async Task<int> CompleteAsync()
        {
            //
            var effectedRows = await _context.SaveChangesAsync();
            return effectedRows;
        }
        public async Task<List<T>> ToListAsync<T>(IQueryable<T> query)
        {
            try
            {
                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<T> ToSingleItem<T>(IQueryable<T> query)
        {
            try
            {
                return await query.FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<T>> ToEnumerable<T>(IQueryable<T> query)
        {
            if (query != null)
                return query.AsEnumerable();
            else
                //TODO: to be replaced after adding logging layer
                throw new System.ArgumentNullException();
        }

        public async Task<T[]> ToArrayAsync<T>(IQueryable<T> query)
        {
            if (query != null)
                return await query.ToArrayAsync();
            else
                //TODO: to be replaced after adding logging layer
                throw new System.ArgumentNullException();
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }
        public async Task<int> CountQuery<T>(IQueryable<T> query)
        {
            try
            {
                return await query.CountAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}