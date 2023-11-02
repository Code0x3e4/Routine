﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RoutineApi.Data;

#nullable disable

namespace RoutineApi.Migrations
{
    [DbContext(typeof(RoutineDbContext))]
    partial class RoutineDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.5");

            modelBuilder.Entity("RoutineApi.Entities.Company", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Introduction")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Companies");

                    b.HasData(
                        new
                        {
                            Id = new Guid("49c1a7e1-0c79-4a89-a3d6-a37998fb8000"),
                            Introduction = "Bigggggggggg",
                            Name = "Macrohard"
                        },
                        new
                        {
                            Id = new Guid("49c1a7e1-0c79-4a89-a3d6-a37998fb8001"),
                            Introduction = "Bad Dragon",
                            Name = "Apple"
                        },
                        new
                        {
                            Id = new Guid("49c1a7e1-0c79-4a89-a3d6-a37998fb8002"),
                            Introduction = "Goodly Goods",
                            Name = "Google"
                        },
                        new
                        {
                            Id = new Guid("49c1a7e1-0c79-4a89-a3d6-a37998fb8003"),
                            Introduction = "996 Company",
                            Name = "Alipapa"
                        },
                        new
                        {
                            Id = new Guid("49c1a7e1-0c79-4a89-a3d6-a37998fb8004"),
                            Introduction = "Fubao Company",
                            Name = "PDD"
                        },
                        new
                        {
                            Id = new Guid("49c1a7e1-0c79-4a89-a3d6-a37998fb8e86"),
                            Introduction = "Patriotic Company",
                            Name = "HuaWei"
                        });
                });

            modelBuilder.Entity("RoutineApi.Entities.Employee", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("TEXT");

                    b.Property<string>("EmployeeNo")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("TEXT");

                    b.Property<string>("FirestName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<int>("Gender")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("RoutineApi.Entities.Employee", b =>
                {
                    b.HasOne("RoutineApi.Entities.Company", "Company")
                        .WithMany("Employees")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("RoutineApi.Entities.Company", b =>
                {
                    b.Navigation("Employees");
                });
#pragma warning restore 612, 618
        }
    }
}