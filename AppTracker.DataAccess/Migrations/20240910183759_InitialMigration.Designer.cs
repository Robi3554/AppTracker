﻿// <auto-generated />
using System;
using AppTracker.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AppTracker.DataAccess.Migrations
{
    [DbContext(typeof(AppUsageContext))]
    [Migration("20240910183759_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.8");

            modelBuilder.Entity("AppTracker.DataAccess.Entities.AppUsage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("TEXT");

                    b.Property<TimeSpan>("UsageTime")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("AppUsages");
                });
#pragma warning restore 612, 618
        }
    }
}
