﻿using RiftArena.Models;
using RiftArena.Models.Contexts;
using RiftArena.Models.Services;
using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;

namespace RiftArena.Models.Services
{
    public interface ITournamentService
    {
        Tournament CreateTournament(Tournament tournament, string userID);
        IEnumerable<Tournament> GetAll();
        IEnumerable<Tournament> GetAllUserTournaments(string userID);
        Tournament GetById(int id);
        Tournament UpdateTournament(int id, Tournament tournament, string userID);
        void DeleteTournament(int id, string userID);
        void PublishTournament(int id, string userID);
    }

    public class TournamentService : ITournamentService
    {
        private RiftArenaContext _context;

        public TournamentService(RiftArenaContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Método que retorna todos os torneios existentes
        /// </summary>
        /// <returns>Todos os torneios existentes</returns>
        public IEnumerable<Tournament> GetAll()
        {
            return _context.Tournaments.ToList();
        }
        /// <summary>
        /// Método que retorna todos os torneios criados pelo user logado
        /// </summary>
        /// <param name="userID">Nickname do user logado</param>
        /// <returns>Todos os torneios criados pelo user</returns>
        public IEnumerable<Tournament> GetAllUserTournaments(string userID)
        {
            var myTournaments = new List<Tournament>();
            var allTour = _context.Tournaments.ToList();
            for (int i = 0; i < allTour.Count; i++)
            {
                if(allTour[i].CreatorNickname == userID)
                {
                    myTournaments.Add(allTour[i]);
                }
            }
            return myTournaments;
        }

        /// <summary>
        /// Método que retorna um torneio através de um ID
        /// </summary>
        /// <param name="id">ID do torneio a retornar</param>
        /// <returns>Torneio com ID fornecido</returns>
        public Tournament GetById(int id)
        {
            return _context.Tournaments.Find(id);
        }

        /// <summary>
        /// Método que permite a criação de um torneio
        /// </summary>
        /// <param name="tournament">Torneio a ser criado</param>
        /// <returns>Torneio criado</returns>
        /// <exception cref="AppException">Exceção caso o torneio a criar falhe nas validações</exception>
        public Tournament CreateTournament(Tournament tournament, string userID)
        {
            if (string.IsNullOrEmpty(tournament.Name))
                throw new AppException("Tournament name is required.");
            if (tournament.NumberOfTeams != 4 && tournament.NumberOfTeams != 8 && tournament.NumberOfTeams != 16)
                throw new AppException("The numbers of teams of the tournament should be 4,8 or 16.");
            if (tournament.Rank == null)
                throw new AppException("Choose a rank.");
            if (tournament.Date < System.DateTime.Now)
                throw new AppException("Invalid date.");
            if (tournament.Region == null)
                throw new AppException("Choose a region.");


            var user = _context.Users.SingleOrDefault(x => x.Nickname == userID);

                tournament.CreatorNickname = userID;
                tournament.State = Status.NotPublished;
                tournament.Stages = new List<Team>();
            

            _context.Tournaments.Add(tournament);
            _context.SaveChanges();
            return tournament;
        }

        /// <summary>
        /// Método que permite a edição de um torneio
        /// </summary>
        /// <param name="id">Torneio a ser editado</param>
        /// <param name="tournament">Torneio com edições feitas</param>
        /// <param name="userID">ID do utilizador logado</param>
        /// <returns>Torneio editado</returns>
        /// <exception cref="AppException">Exceção caso a equipa a criar falhe nas validações</exception>
        public Tournament UpdateTournament(int id, Tournament tournament, string userID)
        {
            var tournamentSer = _context.Tournaments.Find(id);

            if (tournamentSer == null)
                throw new AppException("Tournament not found!");

            if (tournamentSer.CreatorNickname == userID)
            {
                if (tournamentSer.State == Status.NotPublished)
                {
                    tournamentSer.Name = tournament.Name;
                    tournamentSer.Description = tournament.Description;
                    if (tournament.NumberOfTeams != 4 && tournament.NumberOfTeams != 8 && tournament.NumberOfTeams != 16)
                    {
                        throw new AppException("The numbers of teams of the tournament should be 4, 8 or 16.");
                    }
                    else
                    {
                        tournamentSer.NumberOfTeams = tournament.NumberOfTeams;
                    }
                    tournamentSer.Rank = tournament.Rank;
                    if (tournament.Date > System.DateTime.Now || tournamentSer.Date > System.DateTime.Now)
                    {
                        tournamentSer.Date = tournament.Date;
                    }
                    else
                    {
                        throw new AppException("Invalid date.");
                    }
                    if (tournament.Poster != tournamentSer.Poster)
                    {
                        if (File.Exists(tournamentSer.Poster))
                        {
                            File.Delete(tournamentSer.Poster);
                        }
                        tournamentSer.Poster = tournament.Poster;
                    }
                }
                else
                {
                    throw new AppException("Tournament already published");
                }
            }
            else
            {
                throw new AppException("User logged in is not the tournament creator.");
            }

            _context.Tournaments.Update(tournamentSer);
            _context.SaveChanges();
            return GetById(tournamentSer.TournamentId);
        }

        /// <summary>
        /// Método que permite a eliminação de um torneio
        /// </summary>
        /// <param name="id">ID do torneio a eliminar</param>
        /// <param name="userID">ID do utilizador logado</param>
        public void DeleteTournament(int id, string userID)
        {
            var tournament = _context.Tournaments.Find(id);
            if (tournament != null && tournament.CreatorNickname == userID)
            {
                if (File.Exists(tournament.Poster))
                {
                    File.Delete(tournament.Poster);
                }
                _context.Tournaments.Remove(tournament);
                _context.SaveChanges();
            }
            else
            {
                throw new AppException("User logged in is not the tournament creator.");
            }
        }

        /// <summary>
        /// Método que permite a publicação de um torneio
        /// </summary>
        /// <param name="id">ID do torneio a ser publicado</param>
        /// <param name="userID">ID do utilizador logado</param>
        public void PublishTournament(int id, string userID)
        {
            var tournament = _context.Tournaments.Find(id);
            if (tournament.CreatorNickname == userID)
            {
                tournament.State = Status.Published;
            }
            else
            {
                throw new AppException("User logged in is not the tournament creator.");
            }
        }
    }
}
