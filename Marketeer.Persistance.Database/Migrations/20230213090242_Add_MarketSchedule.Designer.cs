﻿// <auto-generated />
using System;
using Marketeer.Persistance.Database.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Marketeer.Persistance.Database.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230213090242_Add_MarketSchedule")]
    partial class Add_MarketSchedule
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Marketeer.Core.Domain.Entities.Auth.AppRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Marketeer.Core.Domain.Entities.Auth.AppRoleClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Marketeer.Core.Domain.Entities.Auth.AppUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Marketeer.Core.Domain.Entities.Auth.AppUserClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Marketeer.Core.Domain.Entities.Auth.AppUserLogin", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Marketeer.Core.Domain.Entities.Auth.AppUserRole", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Marketeer.Core.Domain.Entities.Auth.AppUserToken", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Marketeer.Core.Domain.Entities.Logging.AppLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("EventId")
                        .HasColumnType("int");

                    b.Property<string>("EventName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("LogLevel")
                        .HasColumnType("int");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Source")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StackTrace")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("AppLogs");
                });

            modelBuilder.Entity("Marketeer.Core.Domain.Entities.Logging.CronLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsCanceled")
                        .HasColumnType("bit");

                    b.Property<string>("Message")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("CronLogs");
                });

            modelBuilder.Entity("Marketeer.Core.Domain.Entities.Logging.PythonLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Error")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("File")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Output")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("PythonLogs");
                });

            modelBuilder.Entity("Marketeer.Core.Domain.Entities.Market.HistoryData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<decimal>("Close")
                        .HasPrecision(28, 10)
                        .HasColumnType("decimal(28,10)");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("High")
                        .HasPrecision(28, 10)
                        .HasColumnType("decimal(28,10)");

                    b.Property<int>("Interval")
                        .HasColumnType("int");

                    b.Property<decimal>("Low")
                        .HasPrecision(28, 10)
                        .HasColumnType("decimal(28,10)");

                    b.Property<decimal>("Open")
                        .HasPrecision(28, 10)
                        .HasColumnType("decimal(28,10)");

                    b.Property<int>("TickerId")
                        .HasColumnType("int");

                    b.Property<long>("Volume")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("TickerId", "Interval", "DateTime")
                        .IsUnique();

                    b.ToTable("HistoryDatas");
                });

            modelBuilder.Entity("Marketeer.Core.Domain.Entities.Market.JsonTickerInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("InfoJson")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("Id");

                    b.ToTable("JsonTickerInfos");
                });

            modelBuilder.Entity("Marketeer.Core.Domain.Entities.Market.MarketSchedule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("Day")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("MarketClose")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("MarketOpen")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("MarketSchedules");
                });

            modelBuilder.Entity("Marketeer.Core.Domain.Entities.Market.Ticker", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.HasKey("Id");

                    b.HasIndex("Symbol")
                        .IsUnique();

                    b.ToTable("Tickers");
                });

            modelBuilder.Entity("Marketeer.Core.Domain.Entities.Market.TickerInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<float?>("DividendRate")
                        .HasColumnType("real");

                    b.Property<string>("Exchange")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Industry")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("MarketCap")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float?>("PayoutRatio")
                        .HasColumnType("real");

                    b.Property<string>("QuoteType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Sector")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TickerId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<long?>("Volume")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("TickerId")
                        .IsUnique();

                    b.ToTable("TickerInfos");
                });

            modelBuilder.Entity("Marketeer.Core.Domain.Entities.Market.TickerSetting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("IsDelisted")
                        .HasColumnType("bit");

                    b.Property<int>("TickerId")
                        .HasColumnType("int");

                    b.Property<bool>("UpdateDailyHistory")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("TickerId")
                        .IsUnique();

                    b.ToTable("TickerSettings");
                });

            modelBuilder.Entity("Marketeer.Core.Domain.Entities.Market.TickerSettingHistoryDisable", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Interval")
                        .HasColumnType("int");

                    b.Property<int>("TickerSettingId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TickerSettingId");

                    b.ToTable("TickerSettingHistoryDisables");
                });

            modelBuilder.Entity("Marketeer.Core.Domain.Entities.Auth.AppRoleClaim", b =>
                {
                    b.HasOne("Marketeer.Core.Domain.Entities.Auth.AppRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Marketeer.Core.Domain.Entities.Auth.AppUserClaim", b =>
                {
                    b.HasOne("Marketeer.Core.Domain.Entities.Auth.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Marketeer.Core.Domain.Entities.Auth.AppUserLogin", b =>
                {
                    b.HasOne("Marketeer.Core.Domain.Entities.Auth.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Marketeer.Core.Domain.Entities.Auth.AppUserRole", b =>
                {
                    b.HasOne("Marketeer.Core.Domain.Entities.Auth.AppRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Marketeer.Core.Domain.Entities.Auth.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Marketeer.Core.Domain.Entities.Auth.AppUserToken", b =>
                {
                    b.HasOne("Marketeer.Core.Domain.Entities.Auth.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Marketeer.Core.Domain.Entities.Market.HistoryData", b =>
                {
                    b.HasOne("Marketeer.Core.Domain.Entities.Market.Ticker", "Ticker")
                        .WithMany("HistoryDatas")
                        .HasForeignKey("TickerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ticker");
                });

            modelBuilder.Entity("Marketeer.Core.Domain.Entities.Market.TickerInfo", b =>
                {
                    b.HasOne("Marketeer.Core.Domain.Entities.Market.Ticker", "Ticker")
                        .WithOne("TickerInfo")
                        .HasForeignKey("Marketeer.Core.Domain.Entities.Market.TickerInfo", "TickerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ticker");
                });

            modelBuilder.Entity("Marketeer.Core.Domain.Entities.Market.TickerSetting", b =>
                {
                    b.HasOne("Marketeer.Core.Domain.Entities.Market.Ticker", "Ticker")
                        .WithOne("TickerSetting")
                        .HasForeignKey("Marketeer.Core.Domain.Entities.Market.TickerSetting", "TickerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ticker");
                });

            modelBuilder.Entity("Marketeer.Core.Domain.Entities.Market.TickerSettingHistoryDisable", b =>
                {
                    b.HasOne("Marketeer.Core.Domain.Entities.Market.TickerSetting", "TickerSettings")
                        .WithMany("TempHistoryDisable")
                        .HasForeignKey("TickerSettingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TickerSettings");
                });

            modelBuilder.Entity("Marketeer.Core.Domain.Entities.Market.Ticker", b =>
                {
                    b.Navigation("HistoryDatas");

                    b.Navigation("TickerInfo")
                        .IsRequired();

                    b.Navigation("TickerSetting")
                        .IsRequired();
                });

            modelBuilder.Entity("Marketeer.Core.Domain.Entities.Market.TickerSetting", b =>
                {
                    b.Navigation("TempHistoryDisable");
                });
#pragma warning restore 612, 618
        }
    }
}
