﻿// <auto-generated />
using System;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(MovieTheaterContext))]
    [Migration("20220419124739_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.3");

            modelBuilder.Entity("Infrastructure.Identity.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("PasswordHash")
                        .HasColumnType("BLOB");

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("BLOB");

                    b.Property<string>("Username")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MovieTheaterCore.Models.Actor", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("MovieId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("MovieId");

                    b.ToTable("Actors");
                });

            modelBuilder.Entity("MovieTheaterCore.Models.Director", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("MovieId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("MovieId");

                    b.ToTable("Directors");
                });

            modelBuilder.Entity("MovieTheaterCore.Models.Movie", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AgeRating")
                        .HasColumnType("INTEGER");

                    b.Property<int>("AllowedViewings")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Genre")
                        .HasColumnType("TEXT");

                    b.Property<int>("ReleaseYear")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Runtime")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SpokenLanguage")
                        .HasColumnType("TEXT");

                    b.Property<string>("TextLanguage")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Movies");
                });

            modelBuilder.Entity("MovieTheaterCore.Models.MovieViewing", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("MovieId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("SalonId")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("TicketPrice")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ViewingStart")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("MovieId");

                    b.HasIndex("SalonId");

                    b.ToTable("Viewings");
                });

            modelBuilder.Entity("MovieTheaterCore.Models.Reservation", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("MovieViewingId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ReservationCode")
                        .HasColumnType("TEXT");

                    b.Property<int>("Seats")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("MovieViewingId");

                    b.ToTable("Reservations");
                });

            modelBuilder.Entity("MovieTheaterCore.Models.Review", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("MovieId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ReviewContent")
                        .HasMaxLength(2000)
                        .HasColumnType("TEXT");

                    b.Property<string>("ReviewerName")
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<int>("Score")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("MovieTheaterCore.Models.Salon", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int>("Seats")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Salons");
                });

            modelBuilder.Entity("MovieTheaterCore.Models.Actor", b =>
                {
                    b.HasOne("MovieTheaterCore.Models.Movie", null)
                        .WithMany("Actors")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MovieTheaterCore.Models.Director", b =>
                {
                    b.HasOne("MovieTheaterCore.Models.Movie", null)
                        .WithMany("Directors")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MovieTheaterCore.Models.MovieViewing", b =>
                {
                    b.HasOne("MovieTheaterCore.Models.Movie", "Movie")
                        .WithMany()
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MovieTheaterCore.Models.Salon", "Salon")
                        .WithMany()
                        .HasForeignKey("SalonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Movie");

                    b.Navigation("Salon");
                });

            modelBuilder.Entity("MovieTheaterCore.Models.Reservation", b =>
                {
                    b.HasOne("MovieTheaterCore.Models.MovieViewing", "MovieViewing")
                        .WithMany()
                        .HasForeignKey("MovieViewingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MovieViewing");
                });

            modelBuilder.Entity("MovieTheaterCore.Models.Movie", b =>
                {
                    b.Navigation("Actors");

                    b.Navigation("Directors");
                });
#pragma warning restore 612, 618
        }
    }
}
