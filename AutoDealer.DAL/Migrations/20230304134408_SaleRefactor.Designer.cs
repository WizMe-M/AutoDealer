﻿// <auto-generated />
using System;
using AutoDealer.DAL.Database;
using AutoDealer.DAL.Database.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AutoDealer.DAL.Migrations
{
    [DbContext(typeof(AutoDealerContext))]
    [Migration("20230304134408_SaleRefactor")]
    partial class SaleRefactor
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "auto_status", new[] { "assembled", "testing", "selling", "sold" });
            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "log_type", new[] { "error", "normal" });
            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "post", new[] { "database_admin", "assembly_chief", "purchase_specialist", "storekeeper", "seller", "tester" });
            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "test_status", new[] { "not_checked", "certified", "defective" });
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AutoDealer.DAL.Database.Entity.Auto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id_auto");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateOnly>("AssemblyDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("date")
                        .HasColumnName("assembly_date")
                        .HasDefaultValueSql("now()");

                    b.Property<decimal>("Cost")
                        .HasColumnType("numeric")
                        .HasColumnName("cost");

                    b.Property<int>("IdCarModel")
                        .HasColumnType("integer")
                        .HasColumnName("id_trim");

                    b.Property<AutoStatus>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("auto_status")
                        .HasColumnName("status")
                        .HasDefaultValueSql("'assembled'");

                    b.HasKey("Id")
                        .HasName("pk_autos");

                    b.HasIndex("IdCarModel");

                    b.ToTable("autos", (string)null);
                });

            modelBuilder.Entity("AutoDealer.DAL.Database.Entity.CarModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id_car_model");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("LineName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("line_name");

                    b.Property<string>("ModelName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("model_name");

                    b.Property<string>("TrimCode")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("trim_code");

                    b.HasKey("Id")
                        .HasName("pk_car_models");

                    b.HasIndex(new[] { "LineName", "ModelName", "TrimCode" }, "uq_car_models_name")
                        .IsUnique();

                    b.ToTable("car_models", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            LineName = "Sun",
                            ModelName = "Crawler",
                            TrimCode = "SC-4"
                        });
                });

            modelBuilder.Entity("AutoDealer.DAL.Database.Entity.CarModelDetail", b =>
                {
                    b.Property<int>("IdCarModel")
                        .HasColumnType("integer")
                        .HasColumnName("id_car_model");

                    b.Property<int>("IdDetailSeries")
                        .HasColumnType("integer")
                        .HasColumnName("id_detail_series");

                    b.Property<int>("Count")
                        .HasColumnType("integer")
                        .HasColumnName("count");

                    b.HasKey("IdCarModel", "IdDetailSeries")
                        .HasName("pk_car_model_details");

                    b.HasIndex("IdDetailSeries");

                    b.ToTable("car_model_details", (string)null);
                });

            modelBuilder.Entity("AutoDealer.DAL.Database.Entity.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id_client");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateOnly>("Birthdate")
                        .HasColumnType("date")
                        .HasColumnName("birthdate");

                    b.Property<string>("Birthplace")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("birthplace");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("first_name");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("last_name");

                    b.Property<string>("MiddleName")
                        .HasColumnType("text")
                        .HasColumnName("middle_name");

                    b.Property<string>("PassportDepartmentCode")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("department_code");

                    b.Property<string>("PassportIssuer")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("passport_issuer");

                    b.Property<string>("PassportNumber")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("passport_number");

                    b.Property<string>("PassportSeries")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("passport_series");

                    b.HasKey("Id")
                        .HasName("pk_clients");

                    b.ToTable("clients", (string)null);
                });

            modelBuilder.Entity("AutoDealer.DAL.Database.Entity.Contract", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id_contract");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateOnly>("ConclusionDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("date")
                        .HasColumnName("conclusion_date")
                        .HasDefaultValueSql("now()");

                    b.Property<int?>("IdStorekeeper")
                        .HasColumnType("integer")
                        .HasColumnName("id_storekeeper");

                    b.Property<int?>("IdSupplier")
                        .HasColumnType("integer")
                        .HasColumnName("id_supplier");

                    b.Property<DateOnly?>("LadingBillIssueDate")
                        .HasColumnType("date")
                        .HasColumnName("lading_bill_issue_date");

                    b.Property<DateOnly>("SupplyDate")
                        .HasColumnType("date")
                        .HasColumnName("supply_date");

                    b.Property<decimal>("TotalSum")
                        .HasColumnType("numeric")
                        .HasColumnName("total_sum");

                    b.HasKey("Id")
                        .HasName("pk_contracts");

                    b.HasIndex("IdStorekeeper");

                    b.HasIndex("IdSupplier");

                    b.ToTable("contracts", (string)null);
                });

            modelBuilder.Entity("AutoDealer.DAL.Database.Entity.ContractDetail", b =>
                {
                    b.Property<int>("IdContract")
                        .HasColumnType("integer")
                        .HasColumnName("id_contract");

                    b.Property<int>("IdDetailSeries")
                        .HasColumnType("integer")
                        .HasColumnName("id_detail_series");

                    b.Property<decimal>("CostPerOne")
                        .HasColumnType("numeric")
                        .HasColumnName("cost_per_one");

                    b.Property<int>("Count")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("count")
                        .HasDefaultValueSql("1");

                    b.HasKey("IdContract", "IdDetailSeries")
                        .HasName("pk_contract_details");

                    b.HasIndex("IdDetailSeries");

                    b.ToTable("contract_details", (string)null);
                });

            modelBuilder.Entity("AutoDealer.DAL.Database.Entity.Detail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id_detail");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Cost")
                        .HasColumnType("numeric")
                        .HasColumnName("cost");

                    b.Property<int?>("IdAuto")
                        .HasColumnType("integer")
                        .HasColumnName("id_auto");

                    b.Property<int>("IdContract")
                        .HasColumnType("integer")
                        .HasColumnName("id_contract");

                    b.Property<int>("IdDetailSeries")
                        .HasColumnType("integer")
                        .HasColumnName("id_detail_series");

                    b.HasKey("Id")
                        .HasName("pk_details");

                    b.HasIndex("IdAuto");

                    b.HasIndex("IdContract");

                    b.HasIndex("IdDetailSeries");

                    b.ToTable("details", (string)null);
                });

            modelBuilder.Entity("AutoDealer.DAL.Database.Entity.DetailSeries", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id_detail_series");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("code");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.HasKey("Id")
                        .HasName("pk_detail_series");

                    b.HasIndex(new[] { "Code" }, "uq_detail_series_code")
                        .IsUnique();

                    b.ToTable("detail_series", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Code = "SDH-242-790.1",
                            Description = "Full completed and assembled automobile"
                        });
                });

            modelBuilder.Entity("AutoDealer.DAL.Database.Entity.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id_employee");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("first_name");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("last_name");

                    b.Property<string>("MiddleName")
                        .HasColumnType("text")
                        .HasColumnName("middle_name");

                    b.Property<string>("PassportNumber")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("passport_number");

                    b.Property<string>("PassportSeries")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("passport_series");

                    b.Property<Post>("Post")
                        .HasColumnType("post")
                        .HasColumnName("post");

                    b.HasKey("Id")
                        .HasName("pk_employee");

                    b.ToTable("employees", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            FirstName = "Maxim",
                            LastName = "Timkin",
                            PassportNumber = "975717",
                            PassportSeries = "1199",
                            Post = Post.DatabaseAdmin
                        },
                        new
                        {
                            Id = 2,
                            FirstName = "Ivan",
                            LastName = "Ivanov",
                            PassportNumber = "111111",
                            PassportSeries = "1111",
                            Post = Post.AssemblyChief
                        },
                        new
                        {
                            Id = 3,
                            FirstName = "Andrey",
                            LastName = "Andreev",
                            PassportNumber = "222222",
                            PassportSeries = "2222",
                            Post = Post.PurchaseSpecialist
                        },
                        new
                        {
                            Id = 4,
                            FirstName = "Igor",
                            LastName = "Igorev",
                            PassportNumber = "333333",
                            PassportSeries = "3333",
                            Post = Post.Storekeeper
                        },
                        new
                        {
                            Id = 5,
                            FirstName = "Sergey",
                            LastName = "Sergeev",
                            PassportNumber = "444444",
                            PassportSeries = "4444",
                            Post = Post.Seller
                        },
                        new
                        {
                            Id = 6,
                            FirstName = "Alexey",
                            LastName = "Alexeev",
                            PassportNumber = "555555",
                            PassportSeries = "5555",
                            Post = Post.Tester
                        });
                });

            modelBuilder.Entity("AutoDealer.DAL.Database.Entity.Log", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("log_text");

                    b.Property<DateTime>("Time")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("log_time")
                        .HasDefaultValueSql("now()");

                    b.Property<LogType>("Type")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("log_type")
                        .HasColumnName("log_type")
                        .HasDefaultValueSql("'normal'");

                    b.HasKey("Id")
                        .HasName("logs_pkey");

                    b.ToTable("logs", (string)null);
                });

            modelBuilder.Entity("AutoDealer.DAL.Database.Entity.Margin", b =>
                {
                    b.Property<int>("IdCarModel")
                        .HasColumnType("integer")
                        .HasColumnName("id_trim");

                    b.Property<DateOnly>("StartDate")
                        .HasColumnType("date")
                        .HasColumnName("start_date");

                    b.Property<decimal>("Value")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("numeric")
                        .HasColumnName("margin")
                        .HasDefaultValueSql("10");

                    b.HasKey("IdCarModel", "StartDate")
                        .HasName("pk_margins");

                    b.ToTable("margins", (string)null);
                });

            modelBuilder.Entity("AutoDealer.DAL.Database.Entity.Sale", b =>
                {
                    b.Property<int>("IdAuto")
                        .HasColumnType("integer")
                        .HasColumnName("id_auto");

                    b.Property<DateTime>("ExecutionDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("execution_date")
                        .HasDefaultValueSql("now()");

                    b.Property<int>("IdClient")
                        .HasColumnType("integer")
                        .HasColumnName("id_client");

                    b.Property<int>("IdEmployee")
                        .HasColumnType("integer")
                        .HasColumnName("id_employee");

                    b.Property<decimal>("TotalSum")
                        .HasColumnType("numeric")
                        .HasColumnName("total_sum");

                    b.HasKey("IdAuto", "ExecutionDate")
                        .HasName("pk_sales");

                    b.HasIndex("IdClient");

                    b.HasIndex("IdEmployee");

                    b.ToTable("sales", (string)null);
                });

            modelBuilder.Entity("AutoDealer.DAL.Database.Entity.Supplier", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id_supplier");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CorrespondentAccount")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("correspondent_account");

                    b.Property<string>("LegalAddress")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("legal_address");

                    b.Property<string>("PostalAddress")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("postal_address");

                    b.Property<string>("SettlementAccount")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("settlement_account");

                    b.Property<string>("Tin")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("tin");

                    b.HasKey("Id")
                        .HasName("pk_suppliers");

                    b.ToTable("suppliers", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CorrespondentAccount = "30101810600000000957",
                            LegalAddress = "г. Москва, ул. Ленина, 19",
                            PostalAddress = "г. Ленинград, ул. Дзерджинского, 17б",
                            SettlementAccount = "40817810099910004312",
                            Tin = "123456789000"
                        });
                });

            modelBuilder.Entity("AutoDealer.DAL.Database.Entity.Test", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id_test");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("EmployeeId")
                        .HasColumnType("integer");

                    b.Property<DateOnly?>("EndDate")
                        .HasColumnType("date")
                        .HasColumnName("end_date");

                    b.Property<DateOnly>("StartDate")
                        .HasColumnType("date")
                        .HasColumnName("start_date");

                    b.HasKey("Id")
                        .HasName("pk_tests");

                    b.HasIndex("EmployeeId");

                    b.ToTable("tests", (string)null);
                });

            modelBuilder.Entity("AutoDealer.DAL.Database.Entity.TestAuto", b =>
                {
                    b.Property<int>("IdTest")
                        .HasColumnType("integer")
                        .HasColumnName("id_test");

                    b.Property<int>("IdAuto")
                        .HasColumnType("integer")
                        .HasColumnName("id_auto");

                    b.Property<DateOnly?>("CertificationDate")
                        .HasColumnType("date")
                        .HasColumnName("certification_date");

                    b.Property<TestStatus>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("test_status")
                        .HasColumnName("status")
                        .HasDefaultValueSql("'not_checked'");

                    b.HasKey("IdTest", "IdAuto")
                        .HasName("pk_test_autos");

                    b.HasIndex("IdAuto");

                    b.ToTable("test_autos", (string)null);
                });

            modelBuilder.Entity("AutoDealer.DAL.Database.Entity.User", b =>
                {
                    b.Property<int>("IdEmployee")
                        .HasColumnType("integer")
                        .HasColumnName("id_employee");

                    b.Property<bool>("Deleted")
                        .HasColumnType("boolean")
                        .HasColumnName("deleted");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("password");

                    b.HasKey("IdEmployee")
                        .HasName("pk_users_employees");

                    b.HasIndex(new[] { "Email" }, "uq_users_email")
                        .IsUnique();

                    b.ToTable("users", (string)null);

                    b.HasData(
                        new
                        {
                            IdEmployee = 1,
                            Deleted = false,
                            Email = "db@mail.ru",
                            PasswordHash = "1ED6D5667B292B55FE629FCACB0027C808D6686C8C24B045E15212FC0207C73EBBC97F796695FCD306E2E4D3E8CCBF64C031221403023CEBFE86738119C97C20"
                        },
                        new
                        {
                            IdEmployee = 2,
                            Deleted = false,
                            Email = "chief@mail.ru",
                            PasswordHash = "1ED6D5667B292B55FE629FCACB0027C808D6686C8C24B045E15212FC0207C73EBBC97F796695FCD306E2E4D3E8CCBF64C031221403023CEBFE86738119C97C20"
                        },
                        new
                        {
                            IdEmployee = 3,
                            Deleted = false,
                            Email = "spec@mail.ru",
                            PasswordHash = "1ED6D5667B292B55FE629FCACB0027C808D6686C8C24B045E15212FC0207C73EBBC97F796695FCD306E2E4D3E8CCBF64C031221403023CEBFE86738119C97C20"
                        },
                        new
                        {
                            IdEmployee = 4,
                            Deleted = false,
                            Email = "store@mail.ru",
                            PasswordHash = "1ED6D5667B292B55FE629FCACB0027C808D6686C8C24B045E15212FC0207C73EBBC97F796695FCD306E2E4D3E8CCBF64C031221403023CEBFE86738119C97C20"
                        },
                        new
                        {
                            IdEmployee = 5,
                            Deleted = false,
                            Email = "sell@mail.ru",
                            PasswordHash = "1ED6D5667B292B55FE629FCACB0027C808D6686C8C24B045E15212FC0207C73EBBC97F796695FCD306E2E4D3E8CCBF64C031221403023CEBFE86738119C97C20"
                        },
                        new
                        {
                            IdEmployee = 6,
                            Deleted = false,
                            Email = "test@mail.ru",
                            PasswordHash = "1ED6D5667B292B55FE629FCACB0027C808D6686C8C24B045E15212FC0207C73EBBC97F796695FCD306E2E4D3E8CCBF64C031221403023CEBFE86738119C97C20"
                        });
                });

            modelBuilder.Entity("AutoDealer.DAL.Database.Entity.Auto", b =>
                {
                    b.HasOne("AutoDealer.DAL.Database.Entity.CarModel", "CarModel")
                        .WithMany("Autos")
                        .HasForeignKey("IdCarModel")
                        .IsRequired()
                        .HasConstraintName("fk_autos_trims");

                    b.Navigation("CarModel");
                });

            modelBuilder.Entity("AutoDealer.DAL.Database.Entity.CarModelDetail", b =>
                {
                    b.HasOne("AutoDealer.DAL.Database.Entity.CarModel", "CarModel")
                        .WithMany("CarModelDetails")
                        .HasForeignKey("IdCarModel")
                        .IsRequired()
                        .HasConstraintName("fk_car_model_details_trims");

                    b.HasOne("AutoDealer.DAL.Database.Entity.DetailSeries", "DetailSeries")
                        .WithMany("TrimDetails")
                        .HasForeignKey("IdDetailSeries")
                        .IsRequired()
                        .HasConstraintName("fk_car_model_details_detail_series");

                    b.Navigation("CarModel");

                    b.Navigation("DetailSeries");
                });

            modelBuilder.Entity("AutoDealer.DAL.Database.Entity.Contract", b =>
                {
                    b.HasOne("AutoDealer.DAL.Database.Entity.Employee", "Storekeeper")
                        .WithMany("Contracts")
                        .HasForeignKey("IdStorekeeper")
                        .HasConstraintName("fk_contracts_employees");

                    b.HasOne("AutoDealer.DAL.Database.Entity.Supplier", "Supplier")
                        .WithMany("Contracts")
                        .HasForeignKey("IdSupplier")
                        .HasConstraintName("fk_contracts_suppliers");

                    b.Navigation("Storekeeper");

                    b.Navigation("Supplier");
                });

            modelBuilder.Entity("AutoDealer.DAL.Database.Entity.ContractDetail", b =>
                {
                    b.HasOne("AutoDealer.DAL.Database.Entity.Contract", "Contract")
                        .WithMany("ContractDetails")
                        .HasForeignKey("IdContract")
                        .IsRequired()
                        .HasConstraintName("fk_contract_details_contracts");

                    b.HasOne("AutoDealer.DAL.Database.Entity.DetailSeries", "DetailSeries")
                        .WithMany("ContractDetails")
                        .HasForeignKey("IdDetailSeries")
                        .IsRequired()
                        .HasConstraintName("fk_contract_details_detail_series");

                    b.Navigation("Contract");

                    b.Navigation("DetailSeries");
                });

            modelBuilder.Entity("AutoDealer.DAL.Database.Entity.Detail", b =>
                {
                    b.HasOne("AutoDealer.DAL.Database.Entity.Auto", "Auto")
                        .WithMany("Details")
                        .HasForeignKey("IdAuto")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("fk_details_autos");

                    b.HasOne("AutoDealer.DAL.Database.Entity.Contract", "Contract")
                        .WithMany("Details")
                        .HasForeignKey("IdContract")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_details_contracts");

                    b.HasOne("AutoDealer.DAL.Database.Entity.DetailSeries", "DetailSeries")
                        .WithMany("Details")
                        .HasForeignKey("IdDetailSeries")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_details_detail_series");

                    b.Navigation("Auto");

                    b.Navigation("Contract");

                    b.Navigation("DetailSeries");
                });

            modelBuilder.Entity("AutoDealer.DAL.Database.Entity.Margin", b =>
                {
                    b.HasOne("AutoDealer.DAL.Database.Entity.CarModel", "CarModel")
                        .WithMany("Margins")
                        .HasForeignKey("IdCarModel")
                        .IsRequired()
                        .HasConstraintName("fk_margins_trims");

                    b.Navigation("CarModel");
                });

            modelBuilder.Entity("AutoDealer.DAL.Database.Entity.Sale", b =>
                {
                    b.HasOne("AutoDealer.DAL.Database.Entity.Auto", "Auto")
                        .WithMany("Sales")
                        .HasForeignKey("IdAuto")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_sales_autos");

                    b.HasOne("AutoDealer.DAL.Database.Entity.Client", "Client")
                        .WithMany("Sales")
                        .HasForeignKey("IdClient")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_sales_clients");

                    b.HasOne("AutoDealer.DAL.Database.Entity.Employee", "Employee")
                        .WithMany("Sales")
                        .HasForeignKey("IdEmployee")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired()
                        .HasConstraintName("fk_sales_employees");

                    b.Navigation("Auto");

                    b.Navigation("Client");

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("AutoDealer.DAL.Database.Entity.Test", b =>
                {
                    b.HasOne("AutoDealer.DAL.Database.Entity.Employee", null)
                        .WithMany("Tests")
                        .HasForeignKey("EmployeeId");
                });

            modelBuilder.Entity("AutoDealer.DAL.Database.Entity.TestAuto", b =>
                {
                    b.HasOne("AutoDealer.DAL.Database.Entity.Auto", "Auto")
                        .WithMany("TestAutos")
                        .HasForeignKey("IdAuto")
                        .IsRequired()
                        .HasConstraintName("fk_test_autos_autos");

                    b.HasOne("AutoDealer.DAL.Database.Entity.Test", "Test")
                        .WithMany("TestAutos")
                        .HasForeignKey("IdTest")
                        .IsRequired()
                        .HasConstraintName("fk_test_autos_tests");

                    b.Navigation("Auto");

                    b.Navigation("Test");
                });

            modelBuilder.Entity("AutoDealer.DAL.Database.Entity.User", b =>
                {
                    b.HasOne("AutoDealer.DAL.Database.Entity.Employee", "Employee")
                        .WithOne("User")
                        .HasForeignKey("AutoDealer.DAL.Database.Entity.User", "IdEmployee")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_users_employees");

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("AutoDealer.DAL.Database.Entity.Auto", b =>
                {
                    b.Navigation("Details");

                    b.Navigation("Sales");

                    b.Navigation("TestAutos");
                });

            modelBuilder.Entity("AutoDealer.DAL.Database.Entity.CarModel", b =>
                {
                    b.Navigation("Autos");

                    b.Navigation("CarModelDetails");

                    b.Navigation("Margins");
                });

            modelBuilder.Entity("AutoDealer.DAL.Database.Entity.Client", b =>
                {
                    b.Navigation("Sales");
                });

            modelBuilder.Entity("AutoDealer.DAL.Database.Entity.Contract", b =>
                {
                    b.Navigation("ContractDetails");

                    b.Navigation("Details");
                });

            modelBuilder.Entity("AutoDealer.DAL.Database.Entity.DetailSeries", b =>
                {
                    b.Navigation("ContractDetails");

                    b.Navigation("Details");

                    b.Navigation("TrimDetails");
                });

            modelBuilder.Entity("AutoDealer.DAL.Database.Entity.Employee", b =>
                {
                    b.Navigation("Contracts");

                    b.Navigation("Sales");

                    b.Navigation("Tests");

                    b.Navigation("User");
                });

            modelBuilder.Entity("AutoDealer.DAL.Database.Entity.Supplier", b =>
                {
                    b.Navigation("Contracts");
                });

            modelBuilder.Entity("AutoDealer.DAL.Database.Entity.Test", b =>
                {
                    b.Navigation("TestAutos");
                });
#pragma warning restore 612, 618
        }
    }
}
