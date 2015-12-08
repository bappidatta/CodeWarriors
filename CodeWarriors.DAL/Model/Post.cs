using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeWarriors.DAL.Model
{
    /// <summary>
    /// This is model class for Post
    /// </summary>
    public class Post : Entity
    {
        public string UserId { get; set; }
        public string PostDetails { get; set; }
        public List<string> LikedUserIds { get; set; }
        public List<Comment> Comments { get; set; }
        public string CreatedTime { get; set; }

        public Post()
        {
            this.Comments = new List<Comment>();
            this.LikedUserIds = new List<string>();
        }
    }
}
