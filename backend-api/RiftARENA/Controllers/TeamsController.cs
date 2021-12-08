﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RiftArena.Models.Contexts;
using RiftArena.Models;
using RiftArena.Models.Services;

namespace RiftArena.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly RiftArenaContext _context;
        private readonly ITeamService _service;

        public TeamsController(RiftArenaContext context, ITeamService service)
        {
            _context = context;
            _service = service;
        }


        //POST: api/Teams/createTeam
        [HttpPost("createTeam")]
        public IActionResult CreateTeam([FromBody]Team team)
        {
            try
            {
 
                if (_context.Users.Find(team.TeamLeader) == team.TeamLeader)
                {
                    _service.CreateTeam(team);
                    _context.SaveChanges();
                    return CreatedAtRoute("GetTeam", new { id = team.TeamId }, team);
                }
                else
                {
                    return NotFound();
                }




            }catch (AppException ex)
            {
                return BadRequest(new {message = ex.Message });
            }
        }


        //GET: api/Teams/{id: int}
        [HttpGet("{id}",Name = "GetTeam")]
        public ActionResult<Team> GetByID(int id)
        { 
        
            var teamCon = _service.GetById(id);
            if (teamCon == null)
                return NotFound();
            else
                return teamCon;
          
        }



        //GET: api/Teams 
        [HttpGet(Name = "GetAllTeams")]
        public ActionResult<Team> GetAll()
        {
            var teamsCon = _service.GetAll();
            if (teamsCon == null)
               return NoContent();
            else
                return Ok(teamsCon);
        }

        //[HttpDelete("{id:int}"), Authorize]
        [HttpDelete("{id:int}")]
        public ActionResult<Team> DeleteTeam(int id)
        {
            _service.DeleteTeam(id);

            return Ok();
        }


        //[HttpPut("{id:int}"), Authorize]
        [HttpPut("{id:int}")]
        public IActionResult UpdateTeam(int id, [FromBody] Team team)
        {
            _service.UpdateTeam(id, team);

            _context.Teams.Update(team);
            _context.SaveChanges();

            return NoContent();
            
        }

        [HttpPost("addMember/{id:int}")]
        public ActionResult AddMember(int id, User user)
        {
            _service.AddMember(id, user);

            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("removeMember/{id:int}")]
        public ActionResult RemoveMember(int id, User user)
        {
            _service.RemoveMember(id, user);

            _context.SaveChanges();

            return NoContent() ;    
        }

    }
}
