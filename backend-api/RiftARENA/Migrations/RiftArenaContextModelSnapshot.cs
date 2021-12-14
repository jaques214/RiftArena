﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RiftArena.Models.Contexts;

namespace RiftARENA.Migrations
{
    [DbContext(typeof(RiftArenaContext))]
    partial class RiftArenaContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("RiftArena.Models.LinkedAccount", b =>
            {
                b.Property<int>("ID")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .UseIdentityColumn();

                b.Property<string>("Rank")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Region")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Username")
                    .HasColumnType("nvarchar(max)");

                b.HasKey("ID");

                b.ToTable("LinkedAccounts");
            });

            modelBuilder.Entity("RiftArena.Models.Messages", b =>
            {
                b.Property<int>("MessageId")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .UseIdentityColumn();

                b.Property<string>("Message")
                    .HasColumnType("nvarchar(max)");

                b.Property<int?>("TournamentId")
                    .HasColumnType("int");

                b.Property<int>("UserID")
                    .HasColumnType("int");

                b.HasKey("MessageId");

                b.HasIndex("TournamentId");

                b.ToTable("Messages");
            });

            modelBuilder.Entity("RiftArena.Models.Request", b =>
            {
                b.Property<int>("RequestId")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .UseIdentityColumn();

                b.Property<bool>("Accepted")
                    .HasColumnType("bit");

                b.Property<int?>("TeamId")
                    .HasColumnType("int");

                b.Property<int?>("UserID")
                    .HasColumnType("int");

                    b.HasKey("RequestId");

                b.HasIndex("TeamId");

                    b.HasIndex("UserID");

                b.ToTable("Requests");
            });

            modelBuilder.Entity("RiftArena.Models.Team", b =>
            {
                b.Property<int>("TeamId")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .UseIdentityColumn();

                b.Property<int>("Defeats")
                    .HasColumnType("int");

                b.Property<int>("GamesPlayed")
                    .HasColumnType("int");

                b.Property<string>("Name")
                    .HasColumnType("nvarchar(max)");

                b.Property<int>("NumberMembers")
                    .HasColumnType("int");

                b.Property<string>("Poster")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Rank")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Tag")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("TeamLeader")
                    .HasColumnType("nvarchar(max)");

                b.Property<int>("TournamentsWon")
                    .HasColumnType("int");

                b.Property<int>("Wins")
                    .HasColumnType("int");

                b.HasKey("TeamId");

                b.ToTable("Teams");
            });

            modelBuilder.Entity("RiftArena.Models.Tournament", b =>
            {
                b.Property<int>("TournamentId")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .UseIdentityColumn();

                b.Property<string>("Description")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("FinalWinner")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("MiniumTier")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Name")
                    .HasColumnType("nvarchar(max)");

                b.Property<int>("NumberOfTeams")
                    .HasColumnType("int");

                b.Property<string>("Poster")
                    .HasColumnType("nvarchar(max)");

                b.Property<float>("Prize")
                    .HasColumnType("real");

                b.Property<string>("Rank")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Region")
                    .HasColumnType("nvarchar(max)");

                b.Property<int>("State")
                    .HasColumnType("int");

                b.Property<DateTime>("date")
                    .HasColumnType("datetime2");

                b.HasKey("TournamentId");

                b.ToTable("Tournaments");
            });

            modelBuilder.Entity("RiftArena.Models.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("ContaRiot")
                        .HasColumnType("nvarchar(max)");

                b.Property<string>("Email")
                    .HasColumnType("nvarchar(max)");

                b.Property<int?>("LinkedAccountID")
                    .HasColumnType("int");

                    b.Property<string>("Nickname")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NumVitoriasTotal")
                        .HasColumnType("int");

                b.Property<string>("Password")
                    .HasColumnType("nvarchar(max)");

                b.Property<byte[]>("PasswordHash")
                    .HasColumnType("varbinary(max)");

                b.Property<byte[]>("PasswordSalt")
                    .HasColumnType("varbinary(max)");

                b.Property<string>("Rank")
                    .HasColumnType("nvarchar(max)");

                b.Property<int?>("TeamID")
                    .HasColumnType("int");

                b.Property<string>("Tier")
                    .HasColumnType("nvarchar(max)");

                    b.HasKey("UserID");

                b.HasIndex("LinkedAccountID")
                    .IsUnique()
                    .HasFilter("[LinkedAccountID] IS NOT NULL");

                b.HasIndex("TeamID");

                b.ToTable("Users");
            });

            modelBuilder.Entity("TeamTournament", b =>
            {
                b.Property<int>("StagesTeamId")
                    .HasColumnType("int");

                b.Property<int>("TournamentId")
                    .HasColumnType("int");

                b.HasKey("StagesTeamId", "TournamentId");

                b.HasIndex("TournamentId");

                b.ToTable("TeamTournament");
            });

            modelBuilder.Entity("RiftArena.Models.Messages", b =>
            {
                b.HasOne("RiftArena.Models.Tournament", "Tournament")
                    .WithMany("Chat")
                    .HasForeignKey("TournamentId");

                b.Navigation("Tournament");
            });

            modelBuilder.Entity("RiftArena.Models.Request", b =>
            {
                b.HasOne("RiftArena.Models.Team", "Team")
                    .WithMany()
                    .HasForeignKey("TeamId");

                    b.HasOne("RiftArena.Models.User", "User")
                        .WithMany("Requests")
                        .HasForeignKey("UserID");

                b.Navigation("Team");

                b.Navigation("User");
            });

            modelBuilder.Entity("RiftArena.Models.User", b =>
            {
                b.HasOne("RiftArena.Models.LinkedAccount", "LinkedAccount")
                    .WithOne("User")
                    .HasForeignKey("RiftArena.Models.User", "LinkedAccountID");

                b.HasOne("RiftArena.Models.Team", "Team")
                    .WithMany("Members")
                    .HasForeignKey("TeamID");

                b.Navigation("LinkedAccount");

                b.Navigation("Team");
            });

            modelBuilder.Entity("TeamTournament", b =>
            {
                b.HasOne("RiftArena.Models.Team", null)
                    .WithMany()
                    .HasForeignKey("StagesTeamId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("RiftArena.Models.Tournament", null)
                    .WithMany()
                    .HasForeignKey("TournamentId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            modelBuilder.Entity("RiftArena.Models.LinkedAccount", b =>
            {
                b.Navigation("User");
            });

            modelBuilder.Entity("RiftArena.Models.Team", b =>
            {
                b.Navigation("Members");
            });

            modelBuilder.Entity("RiftArena.Models.Tournament", b =>
            {
                b.Navigation("Chat");
            });

            modelBuilder.Entity("RiftArena.Models.User", b =>
            {
                b.Navigation("Requests");
            });
#pragma warning restore 612, 618
        }
    }
}
