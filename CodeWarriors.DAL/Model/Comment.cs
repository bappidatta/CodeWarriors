using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeWarriors.DAL.Model
{
    /// <summary>
    /// This is model class for comments.
    /// </summary>
    public class Comment
    {
        public string UserId { get; set; }
        public string CommentDetails { get; set; }
        public string CreatedTime { get; set; }
    }
}
