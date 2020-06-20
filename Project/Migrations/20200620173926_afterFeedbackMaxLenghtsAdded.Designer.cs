﻿// <auto-generated />
using System;
using AdvertApi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AdvertApi.Migrations
{
    [DbContext(typeof(AdvertisingDbContext))]
    [Migration("20200620173926_afterFeedbackMaxLenghtsAdded")]
    partial class afterFeedbackMaxLenghtsAdded
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AdvertApi.Entities.Banner", b =>
                {
                    b.Property<int>("IdAdvertisement")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Area")
                        .HasColumnType("decimal(6,2)");

                    b.Property<int>("IdCampaign")
                        .HasColumnType("int");

                    b.Property<int>("Name")
                        .HasColumnType("int");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(6,2)");

                    b.HasKey("IdAdvertisement");

                    b.HasIndex("IdCampaign");

                    b.ToTable("Banner");
                });

            modelBuilder.Entity("AdvertApi.Entities.Building", b =>
                {
                    b.Property<int>("IdBuilding")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<decimal>("Height")
                        .HasColumnType("decimal(6,2)");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<int>("StreetNumber")
                        .HasColumnType("int");

                    b.HasKey("IdBuilding");

                    b.ToTable("Building");

                    b.HasData(
                        new
                        {
                            IdBuilding = 1,
                            City = "Warsaw",
                            Height = 16m,
                            Street = "pretty",
                            StreetNumber = 1
                        },
                        new
                        {
                            IdBuilding = 2,
                            City = "Warsaw",
                            Height = 10m,
                            Street = "pretty",
                            StreetNumber = 2
                        },
                        new
                        {
                            IdBuilding = 3,
                            City = "Warsaw",
                            Height = 7m,
                            Street = "pretty",
                            StreetNumber = 3
                        },
                        new
                        {
                            IdBuilding = 4,
                            City = "Warsaw",
                            Height = 12m,
                            Street = "pretty",
                            StreetNumber = 4
                        },
                        new
                        {
                            IdBuilding = 5,
                            City = "Warsaw",
                            Height = 20m,
                            Street = "nice",
                            StreetNumber = 10
                        },
                        new
                        {
                            IdBuilding = 6,
                            City = "Warsaw",
                            Height = 30m,
                            Street = "nice",
                            StreetNumber = 12
                        },
                        new
                        {
                            IdBuilding = 7,
                            City = "Warsaw",
                            Height = 2m,
                            Street = "nice",
                            StreetNumber = 14
                        });
                });

            modelBuilder.Entity("AdvertApi.Entities.Campaign", b =>
                {
                    b.Property<int>("IdCampaign")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("FromIdBuilding")
                        .HasColumnType("int");

                    b.Property<int>("IdClient")
                        .HasColumnType("int");

                    b.Property<decimal>("PricePerSquareMeter")
                        .HasColumnType("decimal(6,2)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ToIdBuilding")
                        .HasColumnType("int");

                    b.HasKey("IdCampaign");

                    b.HasIndex("FromIdBuilding");

                    b.HasIndex("IdClient");

                    b.HasIndex("ToIdBuilding");

                    b.ToTable("Campaign");
                });

            modelBuilder.Entity("AdvertApi.Entities.Client", b =>
                {
                    b.Property<int>("IdClient")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("RefreshToken")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdClient");

                    b.ToTable("Client");
                });

            modelBuilder.Entity("AdvertApi.Entities.Banner", b =>
                {
                    b.HasOne("AdvertApi.Entities.Campaign", "Campaign")
                        .WithMany("Banners")
                        .HasForeignKey("IdCampaign")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AdvertApi.Entities.Campaign", b =>
                {
                    b.HasOne("AdvertApi.Entities.Building", "Building1")
                        .WithMany("FromIdBuildingCampaigns")
                        .HasForeignKey("FromIdBuilding")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("AdvertApi.Entities.Client", "Client")
                        .WithMany("Campaigns")
                        .HasForeignKey("IdClient")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AdvertApi.Entities.Building", "Building2")
                        .WithMany("ToIdBuildingCampaigns")
                        .HasForeignKey("ToIdBuilding")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
