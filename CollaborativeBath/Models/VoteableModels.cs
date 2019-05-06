using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CollaborativeBath.Models
{
    /// <summary>
    /// Abstract class for a model which can be voted on.
    /// </summary>
    public abstract class Voteable
    {
        /// <summary>
        /// Gets or sets the vote list.
        /// </summary>
        /// <value>
        /// The vote list.
        /// </value>
        [Required]
        public VoteList VoteList { get; set; }
    }

    /// <summary>
    /// A list of votes
    /// </summary>
    public class VoteList
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VoteList"/> class.
        /// </summary>
        public VoteList()
        {
            Votes = new List<Vote>();
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the votes.
        /// </summary>
        /// <value>
        /// The votes.
        /// </value>
        public ICollection<Vote> Votes { get; set; }
        /// <summary>
        /// Gets or sets the subscribed users.
        /// </summary>
        /// <value>
        /// The subscribed users.
        /// </value>
        public ICollection<ApplicationUser> SubscribedUsers { get; set; } = new List<ApplicationUser>();

        /// <summary>
        /// Gets the vote up total.
        /// </summary>
        /// <value>
        /// The vote up.
        /// </value>
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

        /// <summary>
        /// Gets the vote down total.
        /// </summary>
        /// <value>
        /// The vote down.
        /// </value>
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

        /// <summary>
        /// Gets the vote total.
        /// </summary>
        /// <value>
        /// The vote sum.
        /// </value>
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

    /// <summary>
    /// Represents a user's vote
    /// </summary>
    public class Vote
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        public ApplicationUser User { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Vote"/> is an upvote or downvote.
        /// </summary>
        /// <value>
        ///   <c>true</c> if upvote; otherwise, <c>false</c>.
        /// </value>
        public bool Up { get; set; }
    }
}