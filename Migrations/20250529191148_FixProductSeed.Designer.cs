﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SklepGitarowy;

#nullable disable

namespace SklepGitarowy.Migrations
{
    [DbContext(typeof(StoreDbContext))]
    [Migration("20250529191148_FixProductSeed")]
    partial class FixProductSeed
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.5");

            modelBuilder.Entity("SklepGitarowy.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("SklepGitarowy.OrderItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("OrderId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.ToTable("OrderItems");
                });

            modelBuilder.Entity("SklepGitarowy.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Price")
                        .HasColumnType("TEXT");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Gitara akustyczna Yamaha",
                            Price = 999.99m,
                            Quantity = 5
                        },
                        new
                        {
                            Id = 2,
                            Name = "Gitara elektryczna Ibanez",
                            Price = 1499.99m,
                            Quantity = 3
                        },
                        new
                        {
                            Id = 3,
                            Name = "Wzmacniacz Fender",
                            Price = 799.99m,
                            Quantity = 4
                        },
                        new
                        {
                            Id = 4,
                            Name = "Struny D'Addario",
                            Price = 39.99m,
                            Quantity = 10
                        },
                        new
                        {
                            Id = 5,
                            Name = "Pasek gitarowy Ernie Ball",
                            Price = 59.99m,
                            Quantity = 7
                        });
                });

            modelBuilder.Entity("SklepGitarowy.OrderItem", b =>
                {
                    b.HasOne("SklepGitarowy.Order", "Order")
                        .WithMany("Items")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");
                });

            modelBuilder.Entity("SklepGitarowy.Order", b =>
                {
                    b.Navigation("Items");
                });
#pragma warning restore 612, 618
        }
    }
}
