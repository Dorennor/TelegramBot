﻿// <auto-generated />

#nullable disable

using System;
using DesktopApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DesktopApp.Migrations
{
    [DbContext(typeof(TgBotDbContext))]
    [Migration("20220424151406_InitialSQLite")]
    partial class InitialSQLite
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.4");

            modelBuilder.Entity("DesktopApp.Entities.Chat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("ChatId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FirstName")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ChatId")
                        .IsUnique();

                    b.ToTable("Chats");
                });

            modelBuilder.Entity("DesktopApp.Entities.Song", b =>
                {
                    b.Property<int>("Key")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("AddedDateTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("Album")
                        .HasColumnType("TEXT");

                    b.Property<string>("Artist")
                        .HasColumnType("TEXT");

                    b.Property<int>("Duration")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FileId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FileUniqueId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Genres")
                        .HasColumnType("TEXT");

                    b.Property<string>("Performers")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Rating")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Tags")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Year")
                        .HasColumnType("INTEGER");

                    b.HasKey("Key");

                    b.HasIndex("FileUniqueId")
                        .IsUnique();

                    b.ToTable("Songs");
                });
#pragma warning restore 612, 618
        }
    }
}
