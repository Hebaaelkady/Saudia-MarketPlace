dotnet ef dbcontext scaffold "Server=.;User=sa;Password=YOUR_PASSWORD;Database=Production;" -o Entities -f -c DBContext --context-dir ./ Microsoft.EntityFrameworkCore.SqlServer

dotnet ef dbcontext scaffold "Server=YOUR_SERVER;User=YOUR_USER;Password=YOUR_PASSWORD;Database=YOUR_DATABASE;" -o Entities -f -c DBContext --context-dir ./ Microsoft.EntityFrameworkCore.SqlServer


in dbcontext
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
modelBuilder.Entity<Store_shippingUsersReportByDateRange>()
               .HasNoKey(); // Since the stored procedure returns data without a primary key
            modelBuilder.Entity<GetInsideUserViewByDateRange>()
               .HasNoKey(); // Since the stored procedure returns data without a primary key