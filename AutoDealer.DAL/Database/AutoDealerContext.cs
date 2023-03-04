using Npgsql;

namespace AutoDealer.DAL.Database;

public partial class AutoDealerContext : DbContext
{
    protected AutoDealerContext()
    {
    }

    public AutoDealerContext(DbContextOptions<AutoDealerContext> options) : base(options)
    {
    }

    #region Db sets

    public virtual DbSet<Auto> Autos { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Contract> Contracts { get; set; }

    public virtual DbSet<ContractDetail> ContractDetails { get; set; }

    public virtual DbSet<Detail> Details { get; set; }

    public virtual DbSet<DetailSeries> DetailSeries { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<Margin> Margins { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<Test> Tests { get; set; }

    public virtual DbSet<TestAuto> TestAutos { get; set; }

    public virtual DbSet<CarModel> CarModels { get; set; }

    public virtual DbSet<CarModelDetail> TrimDetails { get; set; }

    public virtual DbSet<User> Users { get; set; }

    #endregion

    #region db functions

    public async Task ExecuteSetMarginAsync(int carModelId, DateOnly actsFrom, double margin)
    {
        await Database.ExecuteSqlRawAsync(
            $"select set_margin(trim_id := {carModelId}, begins_act_from := '{actsFrom}', margin_value := {margin});");
    }

    public async Task ExecuteSellAutoAsync(int auto, int client, int employee)
    {
        await Database.ExecuteSqlRawAsync(
            $"select sell_auto(auto := {auto}, client := {client}, employee := {employee});");
    }

    public async Task ExecuteReturnAutoAsync(int auto, DateTime saleTime)
    {
        await Database.ExecuteSqlRawAsync($"select return_auto(auto := {auto}, sale_time := '{saleTime}');");
    }

    public async Task ExecuteProcessLadingBillAsync(int contractId)
    {
        await Database.ExecuteSqlRawAsync($"select process_lading_bill(contract := {contractId})");
    }

    #endregion

    #region config

    public static void ConfigureBuilder(NpgsqlDataSourceBuilder dataSourceBuilder)
    {
        dataSourceBuilder.MapEnum<Post>("post");
        dataSourceBuilder.MapEnum<AutoStatus>("auto_status");
        dataSourceBuilder.MapEnum<TestStatus>("test_status");
        dataSourceBuilder.MapEnum<LogType>("log_type");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresEnum<Post>();
        modelBuilder.HasPostgresEnum<AutoStatus>();
        modelBuilder.HasPostgresEnum<TestStatus>();
        modelBuilder.HasPostgresEnum<LogType>();

        #region configure entities

        modelBuilder.Entity<Auto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_autos");

            entity.ToTable("autos");

            entity.Property(e => e.Id).HasColumnName("id_auto");
            entity.Property(e => e.AssemblyDate)
                .HasColumnName("assembly_date")
                .HasDefaultValueSql("now()");
            entity.Property(e => e.Cost).HasColumnName("cost");
            entity.Property(e => e.IdCarModel).HasColumnName("id_trim");
            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasDefaultValueSql("'assembled'");

            entity.HasOne(d => d.CarModel).WithMany(p => p.Autos)
                .HasForeignKey(d => d.IdCarModel)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_autos_trims");
        });

        modelBuilder.Entity<CarModel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_car_models");

            entity.ToTable("car_models");

            entity.HasIndex(e => new { e.LineName, e.ModelName, Code = e.TrimCode }, "uq_car_models_name").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id_car_model");
            entity.Property(e => e.LineName).HasColumnName("line_name");
            entity.Property(e => e.ModelName).HasColumnName("model_name");
            entity.Property(e => e.TrimCode).HasColumnName("trim_code");
        });

        modelBuilder.Entity<CarModelDetail>(entity =>
        {
            entity.HasKey(e => new { IdTrim = e.IdCarModel, e.IdDetailSeries }).HasName("pk_car_model_details");

            entity.ToTable("car_model_details");

            entity.Property(e => e.IdCarModel).HasColumnName("id_car_model");
            entity.Property(e => e.IdDetailSeries).HasColumnName("id_detail_series");
            entity.Property(e => e.Count).HasColumnName("count");

            entity.HasOne(d => d.DetailSeries).WithMany(p => p.TrimDetails)
                .HasForeignKey(d => d.IdDetailSeries)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_car_model_details_detail_series");

            entity.HasOne(d => d.CarModel).WithMany(p => p.CarModelDetails)
                .HasForeignKey(d => d.IdCarModel)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_car_model_details_trims");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_clients");

            entity.ToTable("clients");

            entity.Property(e => e.Id).HasColumnName("id_client");
            entity.Property(e => e.Birthdate).HasColumnName("birthdate");
            entity.Property(e => e.Birthplace).HasColumnName("birthplace");
            entity.Property(e => e.FirstName).HasColumnName("first_name");
            entity.Property(e => e.LastName).HasColumnName("last_name");
            entity.Property(e => e.MiddleName).HasColumnName("middle_name");
            entity.Property(e => e.PassportDepartmentCode).HasColumnName("department_code");
            entity.Property(e => e.PassportIssuer).HasColumnName("passport_issuer");
            entity.Property(e => e.PassportNumber).HasColumnName("passport_number");
            entity.Property(e => e.PassportSeries).HasColumnName("passport_series");
        });

        modelBuilder.Entity<Contract>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_contracts");

            entity.ToTable("contracts");

            entity.Property(e => e.Id).HasColumnName("id_contract");
            entity.Property(e => e.ConclusionDate)
                .HasDefaultValueSql("now()")
                .HasColumnName("conclusion_date");
            entity.Property(e => e.IdStorekeeper).HasColumnName("id_storekeeper");
            entity.Property(e => e.IdSupplier).HasColumnName("id_supplier");
            entity.Property(e => e.LadingBillIssueDate).HasColumnName("lading_bill_issue_date");
            entity.Property(e => e.SupplyDate).HasColumnName("supply_date");
            entity.Property(e => e.TotalSum).HasColumnName("total_sum");

            entity.HasOne(d => d.Storekeeper).WithMany(p => p.Contracts)
                .HasForeignKey(d => d.IdStorekeeper)
                .HasConstraintName("fk_contracts_employees");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Contracts)
                .HasForeignKey(d => d.IdSupplier)
                .HasConstraintName("fk_contracts_suppliers");
        });

        modelBuilder.Entity<ContractDetail>(entity =>
        {
            entity.HasKey(e => new { e.IdContract, e.IdDetailSeries }).HasName("pk_contract_details");

            entity.ToTable("contract_details");

            entity.Property(e => e.IdContract).HasColumnName("id_contract");
            entity.Property(e => e.IdDetailSeries).HasColumnName("id_detail_series");
            entity.Property(e => e.CostPerOne).HasColumnName("cost_per_one");
            entity.Property(e => e.Count)
                .HasDefaultValueSql("1")
                .HasColumnName("count");

            entity.HasOne(d => d.Contract).WithMany(p => p.ContractDetails)
                .HasForeignKey(d => d.IdContract)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_contract_details_contracts");

            entity.HasOne(d => d.DetailSeries).WithMany(p => p.ContractDetails)
                .HasForeignKey(d => d.IdDetailSeries)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_contract_details_detail_series");
        });

        modelBuilder.Entity<Detail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_details");

            entity.ToTable("details");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id_detail");

            entity.Property(e => e.IdDetailSeries).HasColumnName("id_detail_series");
            entity.Property(e => e.Cost).HasColumnName("cost");
            entity.Property(e => e.IdAuto).HasColumnName("id_auto");
            entity.Property(e => e.IdContract).HasColumnName("id_contract");

            entity.HasOne(d => d.DetailSeries).WithMany(p => p.Details)
                .HasForeignKey(d => d.IdDetailSeries)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_details_detail_series");

            entity.HasOne(d => d.Contract).WithMany(p => p.Details)
                .HasForeignKey(d => d.IdContract)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_details_contracts");

            entity.HasOne(d => d.Auto).WithMany(p => p.Details)
                .HasForeignKey(d => d.IdAuto)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_details_autos");
        });

        modelBuilder.Entity<DetailSeries>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_detail_series");

            entity.ToTable("detail_series");

            entity.HasIndex(e => e.Code, "uq_detail_series_code").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id_detail_series");
            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.Description).HasColumnName("description");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_employee");

            entity.ToTable("employees");

            entity.Property(e => e.Id).HasColumnName("id_employee");
            entity.Property(e => e.FirstName).HasColumnName("first_name");
            entity.Property(e => e.LastName).HasColumnName("last_name");
            entity.Property(e => e.MiddleName).HasColumnName("middle_name");
            entity.Property(e => e.PassportNumber).HasColumnName("passport_number");
            entity.Property(e => e.PassportSeries).HasColumnName("passport_series");
            entity.Property(e => e.Post).HasColumnName("post");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("logs_pkey");

            entity.ToTable("logs");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Text).HasColumnName("log_text");
            entity.Property(e => e.Type).HasColumnName("log_type").HasDefaultValueSql("'normal'");

            entity.Property(e => e.Time)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("log_time");
        });

        modelBuilder.Entity<Margin>(entity =>
        {
            entity.HasKey(e => new { IdTrim = e.IdCarModel, e.StartDate }).HasName("pk_margins");

            entity.ToTable("margins");

            entity.Property(e => e.IdCarModel).HasColumnName("id_trim");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.Value)
                .HasDefaultValueSql("10")
                .HasColumnName("margin");

            entity.HasOne(d => d.CarModel).WithMany(p => p.Margins)
                .HasForeignKey(d => d.IdCarModel)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_margins_trims");
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => new { e.IdAuto, e.ExecutionDate }).HasName("pk_sales");

            entity.ToTable("sales");

            entity.Property(e => e.IdAuto).HasColumnName("id_auto");
            entity.Property(e => e.ExecutionDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("execution_date");
            entity.Property(e => e.IdClient).HasColumnName("id_client");
            entity.Property(e => e.IdEmployee).HasColumnName("id_employee");
            entity.Property(e => e.TotalSum).HasColumnName("total_sum");

            entity.HasOne(d => d.Auto).WithMany(p => p.Sales)
                .HasForeignKey(d => d.IdAuto)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_sales_autos");

            entity.HasOne(d => d.Client).WithMany(p => p.Sales)
                .HasForeignKey(d => d.IdClient)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_sales_clients");

            entity.HasOne(d => d.Employee).WithMany(p => p.Sales)
                .HasForeignKey(d => d.IdEmployee)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_sales_employees");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_suppliers");

            entity.ToTable("suppliers");

            entity.Property(e => e.Id).HasColumnName("id_supplier");
            entity.Property(e => e.CorrespondentAccount).HasColumnName("correspondent_account");
            entity.Property(e => e.LegalAddress).HasColumnName("legal_address");
            entity.Property(e => e.PostalAddress).HasColumnName("postal_address");
            entity.Property(e => e.SettlementAccount).HasColumnName("settlement_account");
            entity.Property(e => e.Tin).HasColumnName("tin");
        });

        modelBuilder.Entity<Test>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_tests");

            entity.ToTable("tests");

            entity.Property(e => e.Id).HasColumnName("id_test");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
        });

        modelBuilder.Entity<TestAuto>(entity =>
        {
            entity.HasKey(e => new { e.IdTest, e.IdAuto }).HasName("pk_test_autos");

            entity.ToTable("test_autos");

            entity.Property(e => e.IdTest).HasColumnName("id_test");
            entity.Property(e => e.IdAuto).HasColumnName("id_auto");
            entity.Property(e => e.CertificationDate).HasColumnName("certification_date");
            entity.Property(e => e.Status).HasColumnName("status").HasDefaultValueSql("'not_checked'");

            entity.HasOne(d => d.Auto).WithMany(p => p.TestAutos)
                .HasForeignKey(d => d.IdAuto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_test_autos_autos");

            entity.HasOne(d => d.Test).WithMany(p => p.TestAutos)
                .HasForeignKey(d => d.IdTest)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_test_autos_tests");
        });


        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdEmployee).HasName("pk_users_employees");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "uq_users_email").IsUnique();

            entity.Property(e => e.IdEmployee)
                .ValueGeneratedNever()
                .HasColumnName("id_employee");
            entity.Property(e => e.Deleted).HasColumnName("deleted");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.PasswordHash).HasColumnName("password");

            entity.HasOne(d => d.Employee).WithOne(p => p.User)
                .HasForeignKey<User>(d => d.IdEmployee)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_users_employees");
        });

        #endregion

        #region preset data

        modelBuilder.Entity<Employee>().HasData(
            new Employee
            {
                Id = 1,
                LastName = "Timkin",
                FirstName = "Maxim",
                PassportSeries = "1199",
                PassportNumber = "975717",
                Post = Post.DatabaseAdmin
            },
            new Employee
            {
                Id = 2,
                LastName = "Ivanov",
                FirstName = "Ivan",
                PassportSeries = "1111",
                PassportNumber = "111111",
                Post = Post.AssemblyChief
            },
            new Employee
            {
                Id = 3,
                LastName = "Andreev",
                FirstName = "Andrey",
                PassportSeries = "2222",
                PassportNumber = "222222",
                Post = Post.PurchaseSpecialist
            },
            new Employee
            {
                Id = 4,
                LastName = "Igorev",
                FirstName = "Igor",
                PassportSeries = "3333",
                PassportNumber = "333333",
                Post = Post.Storekeeper
            },
            new Employee
            {
                Id = 5,
                LastName = "Sergeev",
                FirstName = "Sergey",
                PassportSeries = "4444",
                PassportNumber = "444444",
                Post = Post.Seller
            },
            new Employee
            {
                Id = 6,
                LastName = "Alexeev",
                FirstName = "Alexey",
                PassportSeries = "5555",
                PassportNumber = "555555",
                Post = Post.Tester
            });

        modelBuilder.Entity<User>().HasData(
            new User
            {
                IdEmployee = 1,
                Email = "db@mail.ru",
                // equals to 'password'
                PasswordHash =
                    "1ED6D5667B292B55FE629FCACB0027C808D6686C8C24B045E15212FC0207C73E" +
                    "BBC97F796695FCD306E2E4D3E8CCBF64C031221403023CEBFE86738119C97C20"
            },
            new User
            {
                IdEmployee = 2,
                Email = "chief@mail.ru",
                PasswordHash =
                    "1ED6D5667B292B55FE629FCACB0027C808D6686C8C24B045E15212FC0207C73E" +
                    "BBC97F796695FCD306E2E4D3E8CCBF64C031221403023CEBFE86738119C97C20"
            },
            new User
            {
                IdEmployee = 3,
                Email = "spec@mail.ru",
                PasswordHash =
                    "1ED6D5667B292B55FE629FCACB0027C808D6686C8C24B045E15212FC0207C73E" +
                    "BBC97F796695FCD306E2E4D3E8CCBF64C031221403023CEBFE86738119C97C20"
            },
            new User
            {
                IdEmployee = 4,
                Email = "store@mail.ru",
                PasswordHash =
                    "1ED6D5667B292B55FE629FCACB0027C808D6686C8C24B045E15212FC0207C73E" +
                    "BBC97F796695FCD306E2E4D3E8CCBF64C031221403023CEBFE86738119C97C20"
            },
            new User
            {
                IdEmployee = 5,
                Email = "sell@mail.ru",
                PasswordHash =
                    "1ED6D5667B292B55FE629FCACB0027C808D6686C8C24B045E15212FC0207C73E" +
                    "BBC97F796695FCD306E2E4D3E8CCBF64C031221403023CEBFE86738119C97C20"
            },
            new User
            {
                IdEmployee = 6,
                Email = "test@mail.ru",
                PasswordHash =
                    "1ED6D5667B292B55FE629FCACB0027C808D6686C8C24B045E15212FC0207C73E" +
                    "BBC97F796695FCD306E2E4D3E8CCBF64C031221403023CEBFE86738119C97C20"
            });

        modelBuilder.Entity<CarModel>().HasData(
            new CarModel
            {
                Id = 1,
                LineName = "Sun",
                ModelName = "Crawler",
                TrimCode = "SC-4"
            });

        modelBuilder.Entity<DetailSeries>().HasData(
            new DetailSeries
            {
                Id = 1,
                Code = "SDH-242-790.1",
                Description = "Full completed and assembled automobile"
            });

        modelBuilder.Entity<Supplier>().HasData(
            new Supplier
            {
                Id = 1,
                LegalAddress = "г. Москва, ул. Ленина, 19",
                PostalAddress = "г. Ленинград, ул. Дзерджинского, 17б",
                CorrespondentAccount = "30101810600000000957",
                SettlementAccount = "40817810099910004312",
                Tin = "123456789000"
            });

        #endregion
    }

    #endregion
}