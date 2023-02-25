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

    public virtual DbSet<Auto> Autos { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Contract> Contracts { get; set; }

    public virtual DbSet<ContractDetail> ContractDetails { get; set; }

    public virtual DbSet<Detail> Details { get; set; }

    public virtual DbSet<DetailSeries> DetailSeries { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Line> Lines { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<Margin> Margins { get; set; }

    public virtual DbSet<Model> Models { get; set; }

    public virtual DbSet<PurchaseRequest> PurchaseRequests { get; set; }

    public virtual DbSet<PurchaseRequestDetail> PurchaseRequestDetails { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<Test> Tests { get; set; }

    public virtual DbSet<TestAuto> TestAutos { get; set; }

    public virtual DbSet<Trim> Trims { get; set; }

    public virtual DbSet<TrimDetail> TrimDetails { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Work> Works { get; set; }

    public virtual DbSet<WorkPlan> WorkPlans { get; set; }

    public async Task ExecuteSetMargin(int trimId, DateOnly actsFrom, double margin)
    {
        await Database.ExecuteSqlRawAsync(
            $"select set_margin(trim_id := {trimId}, begins_act_from := '{actsFrom}', margin_value := {margin});");
    }

    public async Task ExecuteAssemblyAuto(int auto)
    {
        await Database.ExecuteSqlRawAsync($"select assembly_auto(auto := {auto});");
    }

    public async Task ExecuteSellAuto(int auto, int client, int employee)
    {
        await Database.ExecuteSqlRawAsync(
            $"select sell_auto(auto := {auto}, client := {client}, employee := {employee});");
    }

    public async Task ExecuteSellAuto(int auto, DateTime saleTime)
    {
        await Database.ExecuteSqlRawAsync($"select return_auto(auto := {auto}, sale_time := '{saleTime}');");
    }

    public static void ConfigureBuilder(NpgsqlDataSourceBuilder dataSourceBuilder)
    {
        dataSourceBuilder.MapEnum<Post>("post");
        dataSourceBuilder.MapEnum<AutoStatus>("auto_status");
        dataSourceBuilder.MapEnum<RequestStatus>("request_status");
        dataSourceBuilder.MapEnum<TestStatus>("test_status");
        dataSourceBuilder.MapEnum<LogType>("log_type");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresEnum<Post>();
        modelBuilder.HasPostgresEnum<AutoStatus>();
        modelBuilder.HasPostgresEnum<RequestStatus>();
        modelBuilder.HasPostgresEnum<TestStatus>();
        modelBuilder.HasPostgresEnum<LogType>();

        modelBuilder.Entity<Auto>(entity =>
        {
            entity.HasKey(e => e.IdAuto).HasName("pk_autos");

            entity.ToTable("autos");

            entity.Property(e => e.IdAuto).HasColumnName("id_auto");
            entity.Property(e => e.AssemblyDate).HasColumnName("assembly_date");
            entity.Property(e => e.Cost).HasColumnName("cost");
            entity.Property(e => e.IdTrim).HasColumnName("id_trim");
            entity.Property(e => e.Status).HasColumnName("status").HasDefaultValueSql("'in_assembly'");

            entity.HasOne(d => d.IdTrimNavigation).WithMany(p => p.Autos)
                .HasForeignKey(d => d.IdTrim)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_autos_trims");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.IdClient).HasName("pk_clients");

            entity.ToTable("clients");

            entity.Property(e => e.IdClient).HasColumnName("id_client");
            entity.Property(e => e.Birthdate).HasColumnName("birthdate");
            entity.Property(e => e.Birthplace).HasColumnName("birthplace");
            entity.Property(e => e.DepartmentCode).HasColumnName("department_code");
            entity.Property(e => e.FirstName).HasColumnName("first_name");
            entity.Property(e => e.LastName).HasColumnName("last_name");
            entity.Property(e => e.MiddleName).HasColumnName("middle_name");
            entity.Property(e => e.PassportIssuer).HasColumnName("passport_issuer");
            entity.Property(e => e.PassportNumber).HasColumnName("passport_number");
            entity.Property(e => e.PassportSeries).HasColumnName("passport_series");
        });

        modelBuilder.Entity<Contract>(entity =>
        {
            entity.HasKey(e => e.IdContract).HasName("pk_contracts");

            entity.ToTable("contracts");

            entity.Property(e => e.IdContract).HasColumnName("id_contract");
            entity.Property(e => e.ConclusionDate)
                .HasDefaultValueSql("now()")
                .HasColumnName("conclusion_date");
            entity.Property(e => e.IdEmployee).HasColumnName("id_employee");
            entity.Property(e => e.IdPurchaseRequest).HasColumnName("id_purchase_request");
            entity.Property(e => e.IdSupplier).HasColumnName("id_supplier");
            entity.Property(e => e.LadingBillIssueDate).HasColumnName("lading_bill_issue_date");
            entity.Property(e => e.SupplyDate).HasColumnName("supply_date");
            entity.Property(e => e.TotalSum).HasColumnName("total_sum");

            entity.HasOne(d => d.IdEmployeeNavigation).WithMany(p => p.Contracts)
                .HasForeignKey(d => d.IdEmployee)
                .HasConstraintName("fk_contracts_employees");

            entity.HasOne(d => d.IdPurchaseRequestNavigation).WithMany(p => p.Contracts)
                .HasForeignKey(d => d.IdPurchaseRequest)
                .HasConstraintName("fk_contracts_purchase_requests");

            entity.HasOne(d => d.IdSupplierNavigation).WithMany(p => p.Contracts)
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

            entity.HasOne(d => d.IdContractNavigation).WithMany(p => p.ContractDetails)
                .HasForeignKey(d => d.IdContract)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_contract_details_contracts");

            entity.HasOne(d => d.IdDetailSeriesNavigation).WithMany(p => p.ContractDetails)
                .HasForeignKey(d => d.IdDetailSeries)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_contract_details_detail_series");
        });

        modelBuilder.Entity<Detail>(entity =>
        {
            entity.HasKey(e => new { e.IdDetailSeries, e.IdDetail }).HasName("pk_details");

            entity.ToTable("details");

            entity.Property(e => e.IdDetailSeries).HasColumnName("id_detail_series");
            entity.Property(e => e.IdDetail)
                .ValueGeneratedOnAdd()
                .HasColumnName("id_detail");
            entity.Property(e => e.Cost).HasColumnName("cost");
            entity.Property(e => e.IdAuto).HasColumnName("id_auto");
            entity.Property(e => e.IdContract).HasColumnName("id_contract");

            entity.HasOne(d => d.IdAutoNavigation).WithMany(p => p.Details)
                .HasForeignKey(d => d.IdAuto)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_details_autos");

            entity.HasOne(d => d.IdContractNavigation).WithMany(p => p.Details)
                .HasForeignKey(d => d.IdContract)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_details_contracts");

            entity.HasOne(d => d.IdDetailSeriesNavigation).WithMany(p => p.Details)
                .HasForeignKey(d => d.IdDetailSeries)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_details_detail_series");
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

        modelBuilder.Entity<Line>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_lines");

            entity.ToTable("lines");

            entity.HasIndex(e => e.Name, "uq_lines_name").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id_line");
            entity.Property(e => e.Name).HasColumnName("name");
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
            entity.HasKey(e => new { e.IdTrim, e.StartDate }).HasName("pk_margins");

            entity.ToTable("margins");

            entity.Property(e => e.IdTrim).HasColumnName("id_trim");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.Margin1)
                .HasDefaultValueSql("10")
                .HasColumnName("margin");

            entity.HasOne(d => d.IdTrimNavigation).WithMany(p => p.Margins)
                .HasForeignKey(d => d.IdTrim)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_margins_trims");
        });

        modelBuilder.Entity<Model>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_models");

            entity.ToTable("models");

            entity.HasIndex(e => e.Name, "uq_models_name").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id_model");
            entity.Property(e => e.IdLine).HasColumnName("id_line");
            entity.Property(e => e.Name).HasColumnName("name");

            entity.HasOne(d => d.Line).WithMany(p => p.Models)
                .HasForeignKey(d => d.IdLine)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_models_lines");
        });

        modelBuilder.Entity<PurchaseRequest>(entity =>
        {
            entity.HasKey(e => e.IdPurchaseRequests).HasName("pk_purchase_requests");

            entity.ToTable("purchase_requests");

            entity.Property(e => e.IdPurchaseRequests).HasColumnName("id_purchase_requests");
            entity.Property(e => e.ExpectedSupplyDate).HasColumnName("expected_supply_date");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.Status).HasColumnName("status")
                .HasDefaultValueSql("'sent'");

            entity.Property(e => e.SentDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("sent_date");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.PurchaseRequests)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_purchase_requests_users");
        });

        modelBuilder.Entity<PurchaseRequestDetail>(entity =>
        {
            entity.HasKey(e => new { e.IdPurchaseRequest, e.IdDetailSeries }).HasName("pk_purchase_request_details");

            entity.ToTable("purchase_request_details");

            entity.Property(e => e.IdPurchaseRequest).HasColumnName("id_purchase_request");
            entity.Property(e => e.IdDetailSeries).HasColumnName("id_detail_series");
            entity.Property(e => e.Count)
                .HasDefaultValueSql("1")
                .HasColumnName("count");

            entity.HasOne(d => d.IdDetailSeriesNavigation).WithMany(p => p.PurchaseRequestDetails)
                .HasForeignKey(d => d.IdDetailSeries)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_purchase_request_details_detail_series");

            entity.HasOne(d => d.IdPurchaseRequestNavigation).WithMany(p => p.PurchaseRequestDetails)
                .HasForeignKey(d => d.IdPurchaseRequest)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_purchase_request_details_purchase_requests");
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

            entity.HasOne(d => d.IdAutoNavigation).WithMany(p => p.Sales)
                .HasForeignKey(d => d.IdAuto)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_sales_autos");

            entity.HasOne(d => d.IdClientNavigation).WithMany(p => p.Sales)
                .HasForeignKey(d => d.IdClient)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_sales_clients");

            entity.HasOne(d => d.IdEmployeeNavigation).WithMany(p => p.Sales)
                .HasForeignKey(d => d.IdEmployee)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_sales_employees");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.IdSupplier).HasName("pk_suppliers");

            entity.ToTable("suppliers");

            entity.Property(e => e.IdSupplier).HasColumnName("id_supplier");
            entity.Property(e => e.CorrespondentAccount).HasColumnName("correspondent_account");
            entity.Property(e => e.LegalAddress).HasColumnName("legal_address");
            entity.Property(e => e.PostalAddress).HasColumnName("postal_address");
            entity.Property(e => e.SettlementAccount).HasColumnName("settlement_account");
            entity.Property(e => e.Tin).HasColumnName("tin");
        });

        modelBuilder.Entity<Test>(entity =>
        {
            entity.HasKey(e => e.IdTest).HasName("pk_tests");

            entity.ToTable("tests");

            entity.Property(e => e.IdTest).HasColumnName("id_test");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.IdEmployee).HasColumnName("id_employee");
            entity.Property(e => e.StartDate).HasColumnName("start_date");

            entity.HasOne(d => d.IdEmployeeNavigation).WithMany(p => p.Tests)
                .HasForeignKey(d => d.IdEmployee)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_tests_employees");
        });

        modelBuilder.Entity<TestAuto>(entity =>
        {
            entity.HasKey(e => new { e.IdTest, e.IdAuto }).HasName("pk_test_autos");

            entity.ToTable("test_autos");

            entity.Property(e => e.IdTest).HasColumnName("id_test");
            entity.Property(e => e.IdAuto).HasColumnName("id_auto");
            entity.Property(e => e.CertificationDate).HasColumnName("certification_date");
            entity.Property(e => e.Status).HasColumnName("status").HasDefaultValueSql("'not_checked'");

            entity.HasOne(d => d.IdAutoNavigation).WithMany(p => p.TestAutos)
                .HasForeignKey(d => d.IdAuto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_test_autos_autos");

            entity.HasOne(d => d.IdTestNavigation).WithMany(p => p.TestAutos)
                .HasForeignKey(d => d.IdTest)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_test_autos_tests");
        });

        modelBuilder.Entity<Trim>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_trims");

            entity.ToTable("trims");

            entity.HasIndex(e => e.Code, "uq_trims_code").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id_trim");
            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.IdModel).HasColumnName("id_model");

            entity.HasOne(d => d.Model).WithMany(p => p.Trims)
                .HasForeignKey(d => d.IdModel)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_trims_models");
        });

        modelBuilder.Entity<TrimDetail>(entity =>
        {
            entity.HasKey(e => new { e.IdTrim, e.IdDetailSeries }).HasName("pk_trim_details");

            entity.ToTable("trim_details");

            entity.Property(e => e.IdTrim).HasColumnName("id_trim");
            entity.Property(e => e.IdDetailSeries).HasColumnName("id_detail_series");
            entity.Property(e => e.Count).HasColumnName("count");

            entity.HasOne(d => d.DetailSeries).WithMany(p => p.TrimDetails)
                .HasForeignKey(d => d.IdDetailSeries)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_trim_details_detail_series");

            entity.HasOne(d => d.Trim).WithMany(p => p.TrimDetails)
                .HasForeignKey(d => d.IdTrim)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_trim_details_trims");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdEmployee).HasName("pk_users_employees");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "uq_users_login").IsUnique();

            entity.Property(e => e.IdEmployee)
                .ValueGeneratedNever()
                .HasColumnName("id_employee");
            entity.Property(e => e.Deleted).HasColumnName("deleted");
            entity.Property(e => e.Email).HasColumnName("login");
            entity.Property(e => e.PasswordHash).HasColumnName("password");

            entity.HasOne(d => d.Employee).WithOne(p => p.User)
                .HasForeignKey<User>(d => d.IdEmployee)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_users_employees");
        });

        modelBuilder.Entity<Work>(entity =>
        {
            entity.HasKey(e => new { e.IdWorkPlan, e.IdAuto }).HasName("pk_works");

            entity.ToTable("works");

            entity.Property(e => e.IdWorkPlan).HasColumnName("id_work_plan");
            entity.Property(e => e.IdAuto).HasColumnName("id_auto");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name).HasColumnName("name");

            entity.HasOne(d => d.IdAutoNavigation).WithMany(p => p.Works)
                .HasForeignKey(d => d.IdAuto)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_works_autos");

            entity.HasOne(d => d.IdWorkPlanNavigation).WithMany(p => p.Works)
                .HasForeignKey(d => d.IdWorkPlan)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_works_work_plans");
        });

        modelBuilder.Entity<WorkPlan>(entity =>
        {
            entity.HasKey(e => e.IdWorkPlan).HasName("pk_work_plans");

            entity.ToTable("work_plans");

            entity.Property(e => e.IdWorkPlan).HasColumnName("id_work_plan");
            entity.Property(e => e.ConclusionDate)
                .HasDefaultValueSql("now()")
                .HasColumnName("conclusion_date");
            entity.Property(e => e.WorkEndDate).HasColumnName("work_end_date");
            entity.Property(e => e.WorkStartDate).HasColumnName("work_start_date");
        });


        modelBuilder.Entity<Employee>().HasData(
            new Employee
            {
                Id = 1,
                LastName = "Timkin",
                FirstName = "Maxim",
                PassportSeries = "1199",
                PassportNumber = "975717",
                Post = Post.DatabaseAdmin
            });
        modelBuilder.Entity<User>().HasData(
            new User
            {
                IdEmployee = 1,
                Email = "timkin.moxim@mail.ru",
                PasswordHash =
                    "1ED6D5667B292B55FE629FCACB0027C808D6686C8C24B045E15212FC0207C73E" +
                    "BBC97F796695FCD306E2E4D3E8CCBF64C031221403023CEBFE86738119C97C20"
            });
    }
}