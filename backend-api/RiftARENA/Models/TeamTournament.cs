using System.ComponentModel.DataAnnotations;

namespace RiftArena.Models
{
    public class TeamTournament
    { 
        public int TeamId {get; set;}
        public int TournamentId {get; set;}
        public int Position {get; set;}
        public override string ToString()
        {
            return base.ToString() + ": " + TeamId.ToString() + ": " + TournamentId.ToString() + ": " + Position.ToString();
        }
    }
}