dotnet ef dbcontext scaffold "Server=.;User=sa;Password=123456;Database=Production;" -o Entities -f -c DBContext --context-dir ./ Microsoft.EntityFrameworkCore.SqlServer

dotnet ef dbcontext scaffold "Server=192.250.231.37;User=egyhubc2_jina;Password=%G3qW5F0Ncj$6i;Database=jina-db;" -o Entities -f -c DBContext --context-dir ./ Microsoft.EntityFrameworkCore.SqlServer


in dbcontext
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
modelBuilder.Entity<Store_shippingUsersReportByDateRange>()
               .HasNoKey(); // Since the stored procedure returns data without a primary key
            modelBuilder.Entity<GetInsideUserViewByDateRange>()
               .HasNoKey(); // Since the stored procedure returns data without a primary key