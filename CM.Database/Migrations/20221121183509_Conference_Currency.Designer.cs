﻿// <auto-generated />
using System;
using CM.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CM.Database.Migrations
{
    [DbContext(typeof(CmDbContext))]
    [Migration("20221121183509_Conference_Currency")]
    partial class Conference_Currency
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("CM.Entities.Club", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("ClubId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<long?>("ClubTypeId")
                        .HasColumnType("bigint");

                    b.Property<string>("Code")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<long?>("CountryId")
                        .HasColumnType("bigint");

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("District")
                        .HasMaxLength(6)
                        .HasColumnType("nvarchar(6)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<long?>("ModifiedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.HasIndex("ClubTypeId");

                    b.HasIndex("CountryId");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("ModifiedBy");

                    b.HasIndex("Name");

                    b.ToTable("Club", (string)null);
                });

            modelBuilder.Entity("CM.Entities.ClubType", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("ClubTypeId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<long?>("ModifiedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("ModifiedBy");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("ClubType", (string)null);
                });

            modelBuilder.Entity("CM.Entities.Conference", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("ConferenceId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("ExchangeRate")
                        .IsRequired()
                        .HasPrecision(10, 4)
                        .HasColumnType("decimal(10,4)");

                    b.Property<long?>("HostClubId")
                        .IsRequired()
                        .HasColumnType("bigint");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<long?>("ModifiedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(75)
                        .HasColumnType("nvarchar(75)");

                    b.Property<long?>("PrimaryCurrencyId")
                        .IsRequired()
                        .HasColumnType("bigint");

                    b.Property<long?>("SecondaryCurrencyId")
                        .IsRequired()
                        .HasColumnType("bigint");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("HostClubId");

                    b.HasIndex("ModifiedBy");

                    b.HasIndex("PrimaryCurrencyId");

                    b.HasIndex("SecondaryCurrencyId");

                    b.ToTable("Conference", (string)null);
                });

            modelBuilder.Entity("CM.Entities.Country", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("CountryId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("nvarchar(3)");

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<long?>("ModifiedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.HasIndex("Code");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("ModifiedBy");

                    b.HasIndex("Name");

                    b.ToTable("Country", (string)null);
                });

            modelBuilder.Entity("CM.Entities.Currency", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("CurrencyId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<long?>("ModifiedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("nvarchar(3)");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("ModifiedBy");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Currency", (string)null);
                });

            modelBuilder.Entity("CM.Entities.Member", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("MemberId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("Alias")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<long?>("ClubId")
                        .IsRequired()
                        .HasColumnType("bigint");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<long?>("ModifiedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("ModifiedBy");

                    b.HasIndex("ClubId", "Name");

                    b.ToTable("Member", (string)null);
                });

            modelBuilder.Entity("CM.Entities.Role", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("RoleId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<long?>("ModifiedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("ModifiedBy");

                    b.ToTable("Role", (string)null);
                });

            modelBuilder.Entity("CM.Entities.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("UserId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<bool?>("EmailConfirmed")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsEnabled")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<long?>("ModifiedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("ModifiedBy");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("CM.Entities.UserPassword", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("UserPasswordId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<long?>("ModifiedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("SecurityStamp")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<long?>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("ModifiedBy");

                    b.HasIndex("UserId");

                    b.ToTable("UserPassword", (string)null);
                });

            modelBuilder.Entity("CM.Entities.UserRefreshToken", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("UserRefreshTokenId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<string>("CreatedByIp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Expires")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<long?>("ModifiedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("ReasonRevoked")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReplacedByToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Revoked")
                        .HasColumnType("datetime2");

                    b.Property<string>("RevokedByIp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("ModifiedBy");

                    b.HasIndex("UserId");

                    b.ToTable("UserRefreshToken", (string)null);
                });

            modelBuilder.Entity("CM.Entities.UserRequestToken", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("UserRequestTokenId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Emitted")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Expires")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<long?>("ModifiedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<long?>("UserId")
                        .IsRequired()
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("Expires");

                    b.HasIndex("ModifiedBy");

                    b.HasIndex("UserId");

                    b.ToTable("UserRequestToken", (string)null);
                });

            modelBuilder.Entity("CM.Entities.UserRole", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("UserRoleId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<long?>("ModifiedBy")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<long>("RoleId")
                        .HasColumnType("bigint");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("ModifiedBy");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId", "RoleId")
                        .IsUnique();

                    b.ToTable("UserRole", (string)null);
                });

            modelBuilder.Entity("CM.Entities.Club", b =>
                {
                    b.HasOne("CM.Entities.ClubType", "ClubType")
                        .WithMany()
                        .HasForeignKey("ClubTypeId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("CM.Entities.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("CM.Entities.User", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedBy");

                    b.HasOne("CM.Entities.User", "ModifiedByUser")
                        .WithMany()
                        .HasForeignKey("ModifiedBy");

                    b.Navigation("ClubType");

                    b.Navigation("Country");

                    b.Navigation("CreatedByUser");

                    b.Navigation("ModifiedByUser");
                });

            modelBuilder.Entity("CM.Entities.ClubType", b =>
                {
                    b.HasOne("CM.Entities.User", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedBy");

                    b.HasOne("CM.Entities.User", "ModifiedByUser")
                        .WithMany()
                        .HasForeignKey("ModifiedBy");

                    b.Navigation("CreatedByUser");

                    b.Navigation("ModifiedByUser");
                });

            modelBuilder.Entity("CM.Entities.Conference", b =>
                {
                    b.HasOne("CM.Entities.User", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedBy");

                    b.HasOne("CM.Entities.Club", "HostClub")
                        .WithMany()
                        .HasForeignKey("HostClubId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CM.Entities.User", "ModifiedByUser")
                        .WithMany()
                        .HasForeignKey("ModifiedBy");

                    b.HasOne("CM.Entities.Currency", "PrimaryCurrency")
                        .WithMany()
                        .HasForeignKey("PrimaryCurrencyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CM.Entities.Currency", "SecondaryCurrency")
                        .WithMany()
                        .HasForeignKey("SecondaryCurrencyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("CreatedByUser");

                    b.Navigation("HostClub");

                    b.Navigation("ModifiedByUser");

                    b.Navigation("PrimaryCurrency");

                    b.Navigation("SecondaryCurrency");
                });

            modelBuilder.Entity("CM.Entities.Country", b =>
                {
                    b.HasOne("CM.Entities.User", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedBy");

                    b.HasOne("CM.Entities.User", "ModifiedByUser")
                        .WithMany()
                        .HasForeignKey("ModifiedBy");

                    b.Navigation("CreatedByUser");

                    b.Navigation("ModifiedByUser");
                });

            modelBuilder.Entity("CM.Entities.Currency", b =>
                {
                    b.HasOne("CM.Entities.User", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedBy");

                    b.HasOne("CM.Entities.User", "ModifiedByUser")
                        .WithMany()
                        .HasForeignKey("ModifiedBy");

                    b.Navigation("CreatedByUser");

                    b.Navigation("ModifiedByUser");
                });

            modelBuilder.Entity("CM.Entities.Member", b =>
                {
                    b.HasOne("CM.Entities.Club", "Club")
                        .WithMany()
                        .HasForeignKey("ClubId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CM.Entities.User", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedBy");

                    b.HasOne("CM.Entities.User", "ModifiedByUser")
                        .WithMany()
                        .HasForeignKey("ModifiedBy");

                    b.Navigation("Club");

                    b.Navigation("CreatedByUser");

                    b.Navigation("ModifiedByUser");
                });

            modelBuilder.Entity("CM.Entities.Role", b =>
                {
                    b.HasOne("CM.Entities.User", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedBy");

                    b.HasOne("CM.Entities.User", "ModifiedByUser")
                        .WithMany()
                        .HasForeignKey("ModifiedBy");

                    b.Navigation("CreatedByUser");

                    b.Navigation("ModifiedByUser");
                });

            modelBuilder.Entity("CM.Entities.User", b =>
                {
                    b.HasOne("CM.Entities.User", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedBy");

                    b.HasOne("CM.Entities.User", "ModifiedByUser")
                        .WithMany()
                        .HasForeignKey("ModifiedBy");

                    b.Navigation("CreatedByUser");

                    b.Navigation("ModifiedByUser");
                });

            modelBuilder.Entity("CM.Entities.UserPassword", b =>
                {
                    b.HasOne("CM.Entities.User", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedBy");

                    b.HasOne("CM.Entities.User", "ModifiedByUser")
                        .WithMany()
                        .HasForeignKey("ModifiedBy");

                    b.HasOne("CM.Entities.User", "User")
                        .WithMany("UserPasswords")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("CreatedByUser");

                    b.Navigation("ModifiedByUser");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CM.Entities.UserRefreshToken", b =>
                {
                    b.HasOne("CM.Entities.User", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedBy");

                    b.HasOne("CM.Entities.User", "ModifiedByUser")
                        .WithMany()
                        .HasForeignKey("ModifiedBy");

                    b.HasOne("CM.Entities.User", "User")
                        .WithMany("UserRefreshToken")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CreatedByUser");

                    b.Navigation("ModifiedByUser");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CM.Entities.UserRequestToken", b =>
                {
                    b.HasOne("CM.Entities.User", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedBy");

                    b.HasOne("CM.Entities.User", "ModifiedByUser")
                        .WithMany()
                        .HasForeignKey("ModifiedBy");

                    b.HasOne("CM.Entities.User", "User")
                        .WithMany("UserRequestTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CreatedByUser");

                    b.Navigation("ModifiedByUser");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CM.Entities.UserRole", b =>
                {
                    b.HasOne("CM.Entities.User", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedBy");

                    b.HasOne("CM.Entities.User", "ModifiedByUser")
                        .WithMany()
                        .HasForeignKey("ModifiedBy");

                    b.HasOne("CM.Entities.Role", "Role")
                        .WithMany("UsersRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CM.Entities.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CreatedByUser");

                    b.Navigation("ModifiedByUser");

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CM.Entities.Role", b =>
                {
                    b.Navigation("UsersRoles");
                });

            modelBuilder.Entity("CM.Entities.User", b =>
                {
                    b.Navigation("UserPasswords");

                    b.Navigation("UserRefreshToken");

                    b.Navigation("UserRequestTokens");

                    b.Navigation("UserRoles");
                });
#pragma warning restore 612, 618
        }
    }
}
