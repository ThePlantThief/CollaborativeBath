using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CollaborativeBath.Models
{
    public abstract class Voteable
    {
        [Required]
        public VoteList VoteList { get; set; }
    }

    public class VoteList
    {
        public VoteList()
        {
            Votes = new List<Vote>();
        }

        public int Id { get; set; }
        public ICollection<Vote> Votes { get; set; }
        public ICollection<ApplicationUser> SubscribedUsers { get; set; } = new List<ApplicationUser>();

        public int VoteUp
        {
            get
            {
                int total = 0;
                foreach (var vote in Votes)
                {
                    if (vote.Up)
                    {
                        total++;
                    }
                }
                return total;
            }
        }

        public int VoteDown
        {
            get
            {
                int total = 0;
                foreach (var vote in Votes)
                {
                    if (!vote.Up)
                    {
                        total++;
                    }
                }
                return total;
            }
        }

        public int VoteSum
        {
            get
            {
                int total = 0;
                foreach (var vote in Votes)
                {
                    if (vote.Up)
                    {
                        total++;
                    }
                    else
                    {
                        total--;
                    }
                }
                return total;
            }
        }
    }

    public class Vote
    {
        public int Id { get; set; }
        public ApplicationUser User { get; set; }
        public bool Up { get; set; }
    }
}