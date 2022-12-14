// <auto-generated />
using System;
using SnoopRatt.App.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace SnoopRatt.App.Migrations
{
    [DbContext(typeof(MaryJaneContext))]
    [Migration("20220729235017_CreateUsersTableAndXpColumn")]
    partial class CreateUsersTableAndXpColumn
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.7");

            modelBuilder.Entity("MaryJaneBud.App.Models.GuildSettings", b =>
                {
                    b.Property<ulong>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<ulong?>("Role")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TimeZone")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("GuildSettings");
                });

            modelBuilder.Entity("MaryJaneBud.App.Models.RoleMention", b =>
                {
                    b.Property<ulong>("Message")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("Channel")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("Guild")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Period")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("TEXT");

                    b.Property<ulong>("User")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Xp")
                        .HasColumnType("REAL");

                    b.HasKey("Message");

                    b.HasIndex("TimeStamp");

                    b.ToTable("RoleMentions");
                });

            modelBuilder.Entity("MaryJaneBud.App.Models.User", b =>
                {
                    b.Property<ulong>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("Xp")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
