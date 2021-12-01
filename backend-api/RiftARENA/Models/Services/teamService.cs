using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using RiftArena.Models;
using RiftArena.Models.Contexts;

namespace RiftArena.Models.Services
{
    public interface ITeamService
    {
        Team CreateTeam(Team team);
        IEnumerable<Team> GetAll();
        Team GetByID(long id);
        Team UpdateTeam(int id,Team team);
        void DeleteTeam(long id);
        void AddMember(long id,User user);
        void RemoveMember(long id, User user);

    }
    public class TeamServices
    {
        private RiftArenaContext _context;

        public TeamServices(RiftArenaContext context)
        {
            _context = context;
        }

        public IEnumerable<Team> GetAll()
        {
            return  _context.Teams.ToList();
        }

        public Team GetByID(long id)
        {
            return _context.Teams.Find(id);
        }

        public Team CreateTeam(Team team)
        {
            if (string.IsNullOrWhiteSpace(team.Name))
                throw new AppException("Team name is required");

            if (_context.Teams.Any(x => x.Name == team.Name))
                throw new AppException("Team name \"" + team.Name + "\" is already taken");

            if (string.IsNullOrWhiteSpace(team.Tag))
                throw new AppException("Team tag is required");

            if (_context.Teams.Any(x => x.Tag == team.Tag))
                throw new AppException("Team tag \"" + team.Tag + "\" is already taken");


            _context.Teams.Add(team);
            _context.SaveChanges();

            return GetByID(team.TeamId);
            
        }

        public Team UpdateTeam(int id,Team team)
        {
            var teamSer = _context.Teams.Find(id);
            if (teamSer != null)
                throw new AppException("Team not found!");


            if (team.Name != teamSer.Name)
            {

                if (_context.Teams.Any(x => x.Name == team.Name))
                    throw new AppException("Team name " + team.Name + " is already taken");
            }

            if (team.Tag != teamSer.Tag)
            {

                if (_context.Teams.Any(x => x.Tag == team.Tag))
                    throw new AppException("Team tag " + team.Tag + " is already taken");
            }

            teamSer.Name = team.Name;
            teamSer.Tag = team.Tag;

            _context.Teams.Update(team);
            _context.SaveChanges();

            return GetByID(team.TeamId);
        }

        public void DeleteTeam(long id)
        {
            var team = _context.Teams.Find(id);
            if (team != null)
            {
                _context.Teams.Remove(team);
                _context.SaveChanges();
            }
        }

        public void AddMember(long id,User user)
        {
            var TeamTemp = GetByID(id);
            if (TeamTemp == null)
            {
                throw new AppException("Not Found");
            }
            else
            {
                if (TeamTemp.NumberMembers == TeamTemp.MAX_MEMBERS)
                {
                    throw new AppException("Team full");
                }
                else
                {
                    TeamTemp.Members.Add(user);
                    TeamTemp.NumberMembers++;
                }
            }
            _context.Teams.Update(TeamTemp);
            _context.SaveChanges();
        }

        public void RemoveMember(long id, User user)
        {
            var TeamTemp = GetByID(id);
            if(TeamTemp == null)
            {
                throw new AppException("Not Found");
            }
            else
            {
                TeamTemp.Members.Remove(user);
                TeamTemp.NumberMembers--;
            }

            _context.Teams.Update(TeamTemp);
            _context.SaveChanges();
        }
    }
}