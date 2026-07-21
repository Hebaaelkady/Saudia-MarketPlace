using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Kader.Data.DataAccessLayer.Entities;
using Kader.Data.DataAccessLayer.Entities.StoredProcedure;

namespace Kader.Data.DataAccessLayer
{
    public partial class DBContext : DbContext
    {
        public DBContext()
        {
        }

        public DBContext(DbContextOptions<DBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<Ads> Ads { get; set; }
        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<BannerImgs> BannerImgs { get; set; }
        public virtual DbSet<CatType> CatType { get; set; }
        public virtual DbSet<Catogry> Catogry { get; set; }
        public virtual DbSet<Cities> Cities { get; set; }
        public virtual DbSet<Colors> Colors { get; set; }
        public virtual DbSet<Governorates> Governorates { get; set; }
        public virtual DbSet<InsideUserView> InsideUserView { get; set; }
        public virtual DbSet<LogPrice> LogPrice { get; set; }
        public virtual DbSet<LogQuantity> LogQuantity { get; set; }
        public virtual DbSet<Neighborhood> Neighborhood { get; set; }
        public virtual DbSet<OrderItems> OrderItems { get; set; }
        public virtual DbSet<OrderStatus> OrderStatus { get; set; }
        public virtual DbSet<Orderss> Orderss { get; set; }
        public virtual DbSet<Pages> Pages { get; set; }
        public virtual DbSet<Payment> Payment { get; set; }
        public virtual DbSet<Paymentss> Paymentss { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductImg> ProductImg { get; set; }
        public virtual DbSet<Reason> Reason { get; set; }
        public virtual DbSet<ReturnsOrder> ReturnsOrder { get; set; }
        public virtual DbSet<ReturnsOrderItem> ReturnsOrderItem { get; set; }
        public virtual DbSet<ReturnsOrderStatus> ReturnsOrderStatus { get; set; }
        public virtual DbSet<ReturnsReason> ReturnsReason { get; set; }
        public virtual DbSet<RoleDetail> RoleDetail { get; set; }
        public virtual DbSet<ShippingPrice> ShippingPrice { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<StoreShippingUsersReport> StoreShippingUsersReport { get; set; }
        public virtual DbSet<Stores> Stores { get; set; }
        public virtual DbSet<TransportMethod> TransportMethod { get; set; }
        public virtual DbSet<Units> Units { get; set; }
        public virtual DbSet<UsersStores> UsersStores { get; set; }
     

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=192.250.231.37;User=egyhubc2_jina;Password=%G3qW5F0Ncj$6i;Database=jina-db;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Store_shippingUsersReportByDateRange>()
               .HasNoKey(); // Since the stored procedure returns data without a primary key
            modelBuilder.Entity<GetInsideUserViewByDateRange>()
               .HasNoKey(); // Since the stored procedure returns data without a primary key
            modelBuilder.Entity<Address>(entity =>
            {
                entity.Property(e => e.AddressId).HasColumnName("AddressID");

                entity.Property(e => e.AddressStreet).HasMaxLength(450);

                entity.Property(e => e.ApartmentNo).HasColumnName("ApartmentNO");

                entity.Property(e => e.BuildingNo)
                    .HasColumnName("BuildingNO")
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.BuildingNo1)
                    .HasColumnName("BuildingNO1")
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.City).HasColumnName("city");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Floor).HasColumnName("floor");

                entity.Property(e => e.GovernoratesId).HasColumnName("GovernoratesID");

                entity.Property(e => e.IdUser).HasMaxLength(450);

                entity.Property(e => e.NeighborhoodId).HasColumnName("NeighborhoodID");

                entity.Property(e => e.SecondryPhone)
                    .HasColumnName("secondryPhone")
                    .HasMaxLength(20);

                entity.Property(e => e.ShiftDelivary).HasDefaultValueSql("((0))");

                entity.Property(e => e.SpecialSign).HasMaxLength(450);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.CityNavigation)
                    .WithMany(p => p.Address)
                    .HasForeignKey(d => d.City)
                    .HasConstraintName("FK_Address_Cities");

                entity.HasOne(d => d.Governorates)
                    .WithMany(p => p.Address)
                    .HasForeignKey(d => d.GovernoratesId)
                    .HasConstraintName("FK_Address_Governorates");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Address)
                    .HasForeignKey(d => d.IdUser)
                    .HasConstraintName("FK_Address_AspNetUsers");

                entity.HasOne(d => d.Neighborhood)
                    .WithMany(p => p.Address)
                    .HasForeignKey(d => d.NeighborhoodId)
                    .HasConstraintName("FK_Address_Neighborhood");
            });

            modelBuilder.Entity<Ads>(entity =>
            {
                entity.Property(e => e.AdsId).HasColumnName("AdsID");

                entity.Property(e => e.AdName1).HasMaxLength(350);

                entity.Property(e => e.AdName2).HasMaxLength(350);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedBy).HasMaxLength(450);

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.InsertedBy).HasMaxLength(450);

                entity.Property(e => e.UpdateBy).HasMaxLength(450);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.Property(e => e.RoleId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => new { e.CatTypeId, e.CatIdaPi, e.NormalizedName })
                    .HasName("IX_AspNetRoles");

                entity.Property(e => e.CatIdaPi).HasColumnName("CatIDaPI");

                entity.Property(e => e.CatTypeId).HasColumnName("CatTypeID");

                entity.Property(e => e.DeletedBy).HasMaxLength(450);

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.IsDelete).HasDefaultValueSql("((0))");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);

                entity.HasOne(d => d.CatType)
                    .WithMany(p => p.AspNetRoles)
                    .HasForeignKey(d => d.CatTypeId)
                    .HasConstraintName("FK_AspNetRoles_CatType");
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.DeletedBy).HasMaxLength(450);

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.RoleId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.Role)
                    .WithMany()
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany()
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.LoginProvider)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany()
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.Property(e => e.ApprovedVerify).HasDefaultValueSql("((0))");

                entity.Property(e => e.CountryCode).HasMaxLength(10);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedBy).HasMaxLength(450);

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.FirstName).HasMaxLength(450);

                entity.Property(e => e.IsDelete).HasDefaultValueSql("((0))");

                entity.Property(e => e.LastName).HasMaxLength(450);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.Status).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<BannerImgs>(entity =>
            {
                entity.HasKey(e => e.BannerImgId);

                entity.Property(e => e.BannerImgId).HasColumnName("BannerImgID");

                entity.Property(e => e.BannerImgName).HasMaxLength(350);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedBy).HasMaxLength(450);

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.InsertedBy).HasMaxLength(450);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.UpdateBy).HasMaxLength(450);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<CatType>(entity =>
            {
                entity.HasKey(e => e.TypeId)
                    .HasName("PK_Type");

                entity.Property(e => e.TypeId).HasColumnName("TypeID");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Catogry>(entity =>
            {
                entity.HasIndex(e => new { e.CatogryName, e.CatId, e.TypeId, e.IsDeleted })
                    .HasName("IX_Catogry")
                    .IsUnique();

                entity.Property(e => e.CatogryId).HasColumnName("CatogryID");

                entity.Property(e => e.CatId).HasColumnName("CatID");

                entity.Property(e => e.CatLevel).HasColumnName("Cat_Level");

                entity.Property(e => e.CatogryName).HasMaxLength(400);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedBy).HasMaxLength(450);

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.InsertedBy).HasMaxLength(450);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.RoleId)
                    .HasColumnName("RoleID")
                    .HasMaxLength(450);

                entity.Property(e => e.TypeId).HasColumnName("TypeID");

                entity.Property(e => e.UpdateBy).HasMaxLength(450);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.Cat)
                    .WithMany(p => p.InverseCat)
                    .HasForeignKey(d => d.CatId)
                    .HasConstraintName("FK_Catogry_Catogry");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Catogry)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_Catogry_AspNetRoles");
            });

            modelBuilder.Entity<Cities>(entity =>
            {
                entity.HasKey(e => e.CityId)
                    .HasName("PK__Cities__F2D21A96D192A51E");

                entity.Property(e => e.CityId)
                    .HasColumnName("CityID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CityName).HasMaxLength(100);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedBy).HasMaxLength(450);

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.GovernorateId).HasColumnName("GovernorateID");

                entity.Property(e => e.InsertedBy).HasMaxLength(450);

                entity.Property(e => e.UpdateBy).HasMaxLength(450);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.Governorate)
                    .WithMany(p => p.Cities)
                    .HasForeignKey(d => d.GovernorateId)
                    .HasConstraintName("FK__Cities__Governor__77DFC722");
            });

            modelBuilder.Entity<Colors>(entity =>
            {
                entity.HasKey(e => e.ColorId);

                entity.Property(e => e.ColorId).HasColumnName("ColorID");

                entity.Property(e => e.ColorDegree).HasMaxLength(150);

                entity.Property(e => e.ColorName).HasMaxLength(150);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedBy).HasMaxLength(450);

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.InsertedBy).HasMaxLength(450);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.UpdateBy).HasMaxLength(450);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Governorates>(entity =>
            {
                entity.HasKey(e => e.GovernorateId)
                    .HasName("PK__Governor__D314ADBAC8E04F93");

                entity.Property(e => e.GovernorateId)
                    .HasColumnName("GovernorateID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedBy).HasMaxLength(450);

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.GovernorateName).HasMaxLength(100);

                entity.Property(e => e.InsertedBy).HasMaxLength(450);

                entity.Property(e => e.UpdateBy).HasMaxLength(450);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<InsideUserView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("InsideUserView");

                entity.Property(e => e.Count1).HasColumnName("COUNT1");

                entity.Property(e => e.CountStatus14).HasColumnName("COUNT_Status_14");

                entity.Property(e => e.FirstName).HasMaxLength(450);

                entity.Property(e => e.LastName).HasMaxLength(450);

                entity.Property(e => e.StoreId).HasColumnName("StoreID");

                entity.Property(e => e.StoreName).HasMaxLength(350);
            });

            modelBuilder.Entity<LogPrice>(entity =>
            {
                entity.HasIndex(e => new { e.DiscountEndDate, e.DiscountBeginDate, e.BeforeDiscount, e.AfterDiscount, e.ProductId })
                    .HasName("IX_LogPrice")
                    .IsUnique();

                entity.Property(e => e.LogPriceId).HasColumnName("LogPriceID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DiscountBeginDate)
                    .HasColumnName("discountBeginDate")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DiscountEndDate)
                    .HasColumnName("discountEndDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.InsertedBy).HasMaxLength(450);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.UpdateBy).HasMaxLength(450);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.LogPrice)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_LogPrice_Product");
            });

            modelBuilder.Entity<LogQuantity>(entity =>
            {
                entity.Property(e => e.LogQuantityId).HasColumnName("LogQuantityID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.InsertedBy).HasMaxLength(450);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.UpdateBy).HasMaxLength(450);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.LogQuantity)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_LogQuantity_Product");
            });

            modelBuilder.Entity<Neighborhood>(entity =>
            {
                entity.Property(e => e.NeighborhoodId).HasColumnName("NeighborhoodID");

                entity.Property(e => e.CityId).HasColumnName("CityID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RegionId).HasColumnName("region_id");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Neighborhood)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("FK_Neighborhood_Cities");
            });

            modelBuilder.Entity<OrderItems>(entity =>
            {
                entity.HasIndex(e => new { e.OrderId, e.ProductId, e.ColorId, e.PriceItem, e.Quantity })
                    .HasName("IX_OrderItems")
                    .IsUnique();

                entity.Property(e => e.OrderItemsId).HasColumnName("OrderItemsID");

                entity.Property(e => e.CatIdApi).HasColumnName("CatIdAPI");

                entity.Property(e => e.ColorId).HasColumnName("ColorID");

                entity.Property(e => e.InsertBy).HasMaxLength(450);

                entity.Property(e => e.InsertDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.ProductName).HasMaxLength(450);

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.Property(e => e.TypeGomlaOrQt3).HasColumnName("typeGomlaOrQt3");

                entity.Property(e => e.UnitId).HasColumnName("UnitID");

                entity.Property(e => e.UpdateBy).HasMaxLength(450);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.Color)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.ColorId)
                    .HasConstraintName("FK_OrderItems_Colors");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderItems_Order");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderItems_Product");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK_OrderItems_Status");

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.UnitId)
                    .HasConstraintName("FK_OrderItems_Units");
            });

            modelBuilder.Entity<OrderStatus>(entity =>
            {
                entity.HasIndex(e => new { e.StoreId, e.OrderId, e.StatusId })
                    .HasName("IX_OrderStatus")
                    .IsUnique();

                entity.Property(e => e.OrderStatusId).HasColumnName("OrderStatusID");

                entity.Property(e => e.Comment).HasMaxLength(450);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedBy).HasMaxLength(450);

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.InsertedBy).HasMaxLength(450);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.Property(e => e.StoreId).HasColumnName("StoreID");

                entity.Property(e => e.UpdateBy).HasMaxLength(450);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderStatus)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_OrderStatus_Orderss");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.OrderStatus)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK_OrderStatus_Status");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.OrderStatus)
                    .HasForeignKey(d => d.StoreId)
                    .HasConstraintName("FK_OrderStatus_Stores");
            });

            modelBuilder.Entity<Orderss>(entity =>
            {
                entity.HasKey(e => e.Idorders)
                    .HasName("PK_Order_1");

                entity.HasIndex(e => e.PaymentId)
                    .HasName("IX_Orderss")
                    .IsUnique();

                entity.Property(e => e.AddressId).HasColumnName("AddressID");

                entity.Property(e => e.CouponDiscount).HasMaxLength(130);

                entity.Property(e => e.InsertDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.InsideShippingUser).HasMaxLength(450);
                entity.Property(e => e.MsgCofeToUser).HasDefaultValueSql("((0))");
                entity.Property(e => e.MsgToDelivery).HasDefaultValueSql("((0))");
                entity.Property(e => e.OrdersNo)
                    .HasColumnName("OrdersNO")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasDefaultValueSql("(abs(checksum(newid()))%(1000000))");

                entity.Property(e => e.PaymentId)
                    .IsRequired()
                    .HasColumnName("PaymentID")
                    .HasMaxLength(250);

                entity.Property(e => e.ShippingDate).HasColumnType("datetime");
                entity.Property(e => e.DeliveryDate).HasColumnType("datetime");
                entity.Property(e => e.ShippingotoId).HasMaxLength(130);

                entity.Property(e => e.ShippingotoMessge).HasMaxLength(130);

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.Property(e => e.StoreId).HasColumnName("StoreID");

                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .HasMaxLength(450);

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.Orderss)
                    .HasForeignKey(d => d.AddressId)
                    .HasConstraintName("FK_Order_Address");

                entity.HasOne(d => d.InsideShippingUserNavigation)
                    .WithMany(p => p.OrderssInsideShippingUserNavigation)
                    .HasForeignKey(d => d.InsideShippingUser)
                    .HasConstraintName("FK_Orderss_AspNetUsers");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Orderss)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK_Orderss_Status");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Orderss)
                    .HasForeignKey(d => d.StoreId)
                    .HasConstraintName("FK_Orderss_Stores");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.OrderssUser)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Order_AspNetUsers");
            });

            modelBuilder.Entity<Pages>(entity =>
            {
                entity.HasKey(e => e.PageId);

                entity.Property(e => e.PageId).HasColumnName("PageID");

                entity.Property(e => e.PageContent).HasColumnType("ntext");

                entity.Property(e => e.PageName).HasMaxLength(150);
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.PId);

                entity.HasIndex(e => e.Id)
                    .HasName("IX_Payment")
                    .IsUnique();

                entity.Property(e => e.PId).HasColumnName("p_id");

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.AuthorizationCode).HasMaxLength(150);

                entity.Property(e => e.Company)
                    .HasColumnName("company")
                    .HasMaxLength(150);

                entity.Property(e => e.Currency)
                    .HasColumnName("currency")
                    .HasMaxLength(150);

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasColumnType("ntext");

                entity.Property(e => e.GatewayId)
                    .HasColumnName("gateway_id")
                    .HasMaxLength(150);

                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasColumnName("id")
                    .HasMaxLength(250);

                entity.Property(e => e.InsertedBy).HasMaxLength(450);

                entity.Property(e => e.InsertedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.InvoiceId)
                    .HasColumnName("invoice_id")
                    .HasMaxLength(150);

                entity.Property(e => e.Ip)
                    .HasColumnName("ip")
                    .HasMaxLength(150);

                entity.Property(e => e.IssuerCardCategory)
                    .HasColumnName("issuer_card_category")
                    .HasMaxLength(150);

                entity.Property(e => e.IssuerCardType).HasMaxLength(150);

                entity.Property(e => e.IssuerName)
                    .HasColumnName("issuer_name")
                    .HasMaxLength(150);

                entity.Property(e => e.Message)
                    .HasColumnName("message")
                    .HasMaxLength(450);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(150);

                entity.Property(e => e.Number)
                    .HasColumnName("number")
                    .HasMaxLength(150);

                entity.Property(e => e.ReferenceNumber)
                    .HasColumnName("reference_number")
                    .HasMaxLength(150);

                entity.Property(e => e.Refunded)
                    .HasColumnName("refunded")
                    .HasMaxLength(150);

                entity.Property(e => e.RefundedAt)
                    .HasColumnName("refunded_at")
                    .HasColumnType("datetime");

                entity.Property(e => e.RefundedFormat)
                    .HasColumnName("refunded_format")
                    .HasMaxLength(150);

                entity.Property(e => e.ResponseCode)
                    .HasColumnName("response_code")
                    .HasMaxLength(150);

                entity.Property(e => e.Status).HasMaxLength(150);

                entity.Property(e => e.Token)
                    .HasColumnName("token")
                    .HasMaxLength(150);

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasMaxLength(150);

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.Payment)
                    .HasPrincipalKey<Orderss>(p => p.PaymentId)
                    .HasForeignKey<Payment>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Payment_Orderss1");

                entity.HasOne(d => d.InsertedByNavigation)
                    .WithMany(p => p.Payment)
                    .HasForeignKey(d => d.InsertedBy)
                    .HasConstraintName("FK_Payment_AspNetUsers");

                entity.HasOne(d => d.StatusOrderNavigation)
                    .WithMany(p => p.PaymentStatusOrderNavigation)
                    .HasForeignKey(d => d.StatusOrder)
                    .HasConstraintName("FK_Payment_Status2");

                entity.HasOne(d => d.StatusOrderItemsNavigation)
                    .WithMany(p => p.PaymentStatusOrderItemsNavigation)
                    .HasForeignKey(d => d.StatusOrderItems)
                    .HasConstraintName("FK_Payment_Status1");
            });

            modelBuilder.Entity<Paymentss>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Amount)
                    .HasColumnName("amount")
                    .HasMaxLength(150);

                entity.Property(e => e.Currency)
                    .HasColumnName("currency")
                    .HasMaxLength(150);

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasColumnType("ntext");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(250);

                entity.Property(e => e.InsertedBy).HasMaxLength(450);

                entity.Property(e => e.InsertedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Message)
                    .HasColumnName("message")
                    .HasMaxLength(450);

                entity.Property(e => e.PId)
                    .HasColumnName("p_id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Status).HasMaxLength(150);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasIndex(e => new { e.CatTypeId, e.Barcode })
                    .HasName("IX_Product")
                    .IsUnique();

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.ApearInHomePage).HasDefaultValueSql("((0))");

                entity.Property(e => e.ApiProdId).HasColumnName("ApiProdID");

                entity.Property(e => e.Barcode)
                    .HasColumnName("barcode")
                    .HasMaxLength(150);

                entity.Property(e => e.CatTypeId).HasColumnName("CatTypeID");

                entity.Property(e => e.CatogryId).HasColumnName("CatogryID");

                entity.Property(e => e.ColorId).HasColumnName("ColorID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedBy).HasMaxLength(450);

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasColumnType("ntext");

                entity.Property(e => e.DiscountBeginDate)
                    .HasColumnName("discountBeginDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.DiscountEndDate)
                    .HasColumnName("discountEndDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.InsertedBy).HasMaxLength(450);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.MaxQuantityToShipQta3a).HasColumnName("MaxQuantityToShip_qta3a");

                entity.Property(e => e.MinQuantityToShipJomla).HasColumnName("MinQuantityToShip_jomla");

                entity.Property(e => e.SpecialOrder).HasDefaultValueSql("((0))");

                entity.Property(e => e.SubCatogryTitle).HasMaxLength(400);

                entity.Property(e => e.SubCatogrysubTitle).HasMaxLength(400);

                entity.Property(e => e.TaxId).HasColumnName("Tax_Id");

                entity.Property(e => e.TransportMethodId).HasColumnName("TransportMethodID");

                entity.Property(e => e.UnitId).HasColumnName("Unit_Id");

                entity.Property(e => e.UnitName)
                    .HasColumnName("Unit_Name")
                    .HasMaxLength(150);

                entity.Property(e => e.UpdateBy).HasMaxLength(450);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.CatType)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.CatTypeId)
                    .HasConstraintName("FK_Product_CatType");

                entity.HasOne(d => d.Catogry)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.CatogryId)
                    .HasConstraintName("FK_SubCatogry_Catogry");

                entity.HasOne(d => d.Color)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.ColorId)
                    .HasConstraintName("FK_Product_Colors");

                entity.HasOne(d => d.TransportMethod)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.TransportMethodId)
                    .HasConstraintName("FK_SubCatogry_SubCatogry");

                entity.HasOne(d => d.UnitNavigation)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.Unit)
                    .HasConstraintName("FK_SubCatogry_Units");
            });

            modelBuilder.Entity<ProductImg>(entity =>
            {
                entity.HasIndex(e => new { e.ProductImgName, e.ProductId })
                    .HasName("IX_ProductImg")
                    .IsUnique();

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedBy).HasMaxLength(450);

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.LastModifiedBy).HasMaxLength(450);

                entity.Property(e => e.LastModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.ProductImgName).HasMaxLength(350);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductImg)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_ProductImg_Product");
            });

            modelBuilder.Entity<Reason>(entity =>
            {
                entity.Property(e => e.ReasonId).HasColumnName("ReasonID");

                entity.Property(e => e.ReasonName).HasMaxLength(250);
            });

            modelBuilder.Entity<ReturnsOrder>(entity =>
            {
                entity.Property(e => e.Comment).HasMaxLength(530);

                entity.Property(e => e.InsertedBy).HasMaxLength(450);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
                entity.Property(e => e.insideShipping).HasDefaultValueSql("((0))");
                entity.Property(e => e.RequestDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ReturnMethod).HasMaxLength(50);

                entity.Property(e => e.ShippingDate).HasColumnType("datetime");

                entity.Property(e => e.ShippingReturnotoMessge).HasMaxLength(130);

                entity.Property(e => e.StatusId)
                    .HasColumnName("StatusID")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.StoreId).HasColumnName("StoreID");

                entity.Property(e => e.UpdateBy).HasMaxLength(450);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.InsertedByNavigation)
                    .WithMany(p => p.ReturnsOrder)
                    .HasForeignKey(d => d.InsertedBy)
                    .HasConstraintName("FK_ReturnsOrder_ReturnsOrder");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.ReturnsOrder)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ReturnsOrder_Orderss");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.ReturnsOrder)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK_ReturnsOrder_Status");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.ReturnsOrder)
                    .HasForeignKey(d => d.StoreId)
                    .HasConstraintName("FK_ReturnsOrder_Stores");
            });

            modelBuilder.Entity<ReturnsOrderItem>(entity =>
            {
                entity.HasIndex(e => e.OrderItemsId)
                    .HasName("IX_ReturnsOrderItem")
                    .IsUnique();

                entity.Property(e => e.ReturnsOrderItemId).HasColumnName("ReturnsOrderItemID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.InsertedBy).HasMaxLength(450);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.StatusId)
                    .HasColumnName("StatusID")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.UpdateBy).HasMaxLength(450);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.OrderItems)
                    .WithOne(p => p.ReturnsOrderItem)
                    .HasForeignKey<ReturnsOrderItem>(d => d.OrderItemsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ReturnsOrderItem_OrderItems");

                entity.HasOne(d => d.ReasonNavigation)
                    .WithMany(p => p.ReturnsOrderItem)
                    .HasForeignKey(d => d.Reason)
                    .HasConstraintName("FK_ReturnsOrderItem_Reason");

                entity.HasOne(d => d.ReturnsOrder)
                    .WithMany(p => p.ReturnsOrderItem)
                    .HasForeignKey(d => d.ReturnsOrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ReturnsOrderItem_ReturnsOrder");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.ReturnsOrderItem)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK_ReturnsOrderItem_Status");
            });

            modelBuilder.Entity<ReturnsOrderStatus>(entity =>
            {
                entity.Property(e => e.ReturnsOrderStatusId).HasColumnName("ReturnsOrderStatusID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedBy).HasMaxLength(450);

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.InsertedBy).HasMaxLength(450);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.ReturnsOrderId).HasColumnName("ReturnsOrderID");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.Property(e => e.StoreId).HasColumnName("StoreID");

                entity.Property(e => e.UpdateBy).HasMaxLength(450);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.ReturnsOrder)
                    .WithMany(p => p.ReturnsOrderStatus)
                    .HasForeignKey(d => d.ReturnsOrderId)
                    .HasConstraintName("FK_ReturnsOrderStatus_ReturnsOrder");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.ReturnsOrderStatus)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK_ReturnsOrderStatus_Status");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.ReturnsOrderStatus)
                    .HasForeignKey(d => d.StoreId)
                    .HasConstraintName("FK_ReturnsOrderStatus_Stores");
            });

            modelBuilder.Entity<ReturnsReason>(entity =>
            {
                entity.Property(e => e.ReturnsReasonId).HasColumnName("ReturnsReasonID");

                entity.Property(e => e.ReturnsReasonName).HasMaxLength(350);
            });

            modelBuilder.Entity<RoleDetail>(entity =>
            {
                entity.HasKey(e => e.RoleDetaailId)
                    .HasName("PK_kader_RoleDetail");

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedBy).HasMaxLength(450);

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.LastModifiedBy1).HasMaxLength(450);

                entity.Property(e => e.LastModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.RoleId).HasMaxLength(450);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.RoleDetail)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_RoleDetail_AspNetRoles");
            });

            modelBuilder.Entity<ShippingPrice>(entity =>
            {
                entity.HasIndex(e => new { e.ProductId, e.VarPrice, e.VarQuantity })
                    .HasName("IX_ShippingPrice")
                    .IsUnique();

                entity.Property(e => e.ShippingPriceId).HasColumnName("ShippingPriceID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedBy).HasMaxLength(450);

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.InsertedBy).HasMaxLength(450);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsOddEven).HasColumnName("IsODD_Even");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.UpdateBy).HasMaxLength(450);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.VarPrice).HasColumnName("var_Price");

                entity.Property(e => e.VarQuantity).HasColumnName("var_Quantity");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ShippingPriceNavigation)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_ShippingPrice_Product");
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.Property(e => e.StatusId).ValueGeneratedNever();

                entity.Property(e => e.StatusName).HasMaxLength(50);
            });

            modelBuilder.Entity<StoreShippingUsersReport>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Store_shippingUsersReport");

                entity.Property(e => e.CountStatus12).HasColumnName("Count_Status_12");

                entity.Property(e => e.CountStatus3).HasColumnName("Count_Status_3");

                entity.Property(e => e.FirstName).HasMaxLength(450);

                entity.Property(e => e.LastName).HasMaxLength(450);

                entity.Property(e => e.RoleName).HasMaxLength(256);

                entity.Property(e => e.StoreId).HasColumnName("StoreID");

                entity.Property(e => e.StoreName).HasMaxLength(350);

                entity.Property(e => e.UserName).HasMaxLength(901);
            });

            modelBuilder.Entity<Stores>(entity =>
            {
                entity.HasKey(e => e.StoreId);

                entity.Property(e => e.StoreId).HasColumnName("StoreID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedBy).HasMaxLength(450);

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.InsertedBy).HasMaxLength(450);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.StoreName).HasMaxLength(350);

                entity.Property(e => e.UpdateBy).HasMaxLength(450);
                entity.Property(e => e.Address).HasMaxLength(450);
                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<TransportMethod>(entity =>
            {
                entity.Property(e => e.TransportMethodId).HasColumnName("TransportMethodID");

                entity.Property(e => e.TransportMethodName).HasMaxLength(150);
            });

            modelBuilder.Entity<Units>(entity =>
            {
                entity.HasKey(e => e.UnitId);

                entity.Property(e => e.UnitId).HasColumnName("UnitID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedBy).HasMaxLength(450);

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.InsertedBy).HasMaxLength(450);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.UnitName)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.UpdateBy).HasMaxLength(450);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<UsersStores>(entity =>
            {
                entity.Property(e => e.UsersStoresId).HasColumnName("UsersStoresID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedBy).HasMaxLength(450);

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.InsertedBy).HasMaxLength(450);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.StoreId).HasColumnName("StoreID");

                entity.Property(e => e.UpdateBy).HasMaxLength(450);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.UsersStores)
                    .HasForeignKey(d => d.StoreId)
                    .HasConstraintName("FK_UsersStores_Stores");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UsersStores)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_UsersStores_AspNetUsers");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
